using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using Edrive.PayPalAPI;

using Customer = Edrive.Models.Customer;

namespace Edrive.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IStateProvinceService _stateProvinceService;

		public RegisterController(IStateProvinceService stateProvinceService)
		{
			_stateProvinceService = stateProvinceService;
		}

		private string ActivationCode
		{
			get { return (Session["NewAccountActivationCode"] ?? String.Empty).ToString(); }
			set { Session["NewAccountActivationCode"] = value; }
		}

		public ActionResult Index()
		{
			try
			{
				ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Register.metatitle");
				ViewData["description"] = SettingManager.GetSettingValue("SEO.Register.description");
				ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Register.keywords");
				var lstOptions = GetOptions();
				ViewData["Options"] = lstOptions;
				ViewData["StateProvince"] = GetStates();

				if(HttpContext.User.Identity.IsAuthenticated)
				{
					using(eDriveAutoWebEntities context = new eDriveAutoWebEntities())
					{
						string email = HttpContext.User.Identity.Name.ToLower();
						var user = context.Customer.FirstOrDefault(c => c.Email.ToLower() == email);

						if(user != null)
						{
							_UserRegisteration model = new _UserRegisteration
							{
								Firstname = user.FirstName,
								Lastname = user.LastName,
								Address = user.ADDRESS,
								Email = user.Email,
								City = user.CITY,
								State = user.STATE,
								Zip = user.zip.GetValueOrDefault(0)
							};

							return View(model);
						}
					}
				}
			}
			catch(Exception ex)
			{
				Edrive.Helpers.Log.Event("Error", ex.Message, "");
			}

			return View();
		}

		public ActionResult RegisterUser()
		{
			ViewData["Msg"] = TempData["Msg"];

			#region for facebook authentication
			const string pageName = "Register/RegisterUser";
			if(User.Identity.IsAuthenticated == false)
			{
				String retUrl = string.Format("{0}{1}&scope={2}", Common_Methods.GetDomainUrl(), pageName, Common_Methods.getFaceBookScope());
				String facebookLoginUrl = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}", Common_Methods.GetFacebookApplicationId(), retUrl);
				ViewData["facebookLoginUrl"] = facebookLoginUrl;
			}
			if(Request.QueryString["code"] != null && User.Identity.IsAuthenticated == false)
			{
				try
				{
					Common_Methods.CreateUserFromFaceBook(pageName);
					return RedirectToAction("Index", "Home");
				}
				catch(Exception ex)
				{
					TempData["Msg"] = "Error" + ex.Message;
				}

				return RedirectToAction("RegisterUser", "Register");
			}
			#endregion

			return View();
		}

		public ActionResult RegisterSuccess(int id, string code = null)
		{
			if(String.IsNullOrWhiteSpace(code) || code.ToLower() == ActivationCode.ToLower())
				return RedirectToAction("Index", "Home"); //Activation Code cannot be null

			var isSuccess = ActivateAccount(id);

			if(isSuccess)
				return RedirectToAction("Success");
			else
				return View("MembershipRenewed");
		}

		public ActionResult Success()
		{
			ViewData["Msg"] = "Your registration completed successfully.";
			return View("MembershipRenewed");
		}

		/// <summary>
		/// It register the Users
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Index(_UserRegisteration userModel)
		{
			if(ModelState.IsValid)
			{
				#region PayPal
				if(userModel.PaymentType == "PayPal")
				{
                    Customer newCustomer = CreateGuestCustomer(userModel);
                    var requesterCredentials = PayPalUtility.BuildPayPalWebservice();

                    SetExpressCheckoutRequestDetailsType details = new SetExpressCheckoutRequestDetailsType();
                    details.NoShipping = "1";

                    PaymentDetailsType payment = new PaymentDetailsType()
                    {
                        OrderDescription = "Full Access Registration (10 days)",
                        OrderTotal = new BasicAmountType()
                        {
                            currencyID = CurrencyCodeType.USD,
                            Value = "9.95"
                        }
                    };

                    details.PaymentDetails = new PaymentDetailsType[]{payment};
					details.CancelURL = string.Format("{0}Register/ConfirmMemberShip?uid={1}&cancel=true", Common_Methods.GetDomainUrl(), newCustomer.CustomerID);
					details.ReturnURL = string.Format("{0}Register/ConfirmMemberShip?uid={1}", Common_Methods.GetDomainUrl(), newCustomer.CustomerID);

					SetExpressCheckoutReq request = new SetExpressCheckoutReq()
                    {
                        SetExpressCheckoutRequest = new SetExpressCheckoutRequestType()
                        {
                            SetExpressCheckoutRequestDetails = details,
                            Version = PayPalUtility.Version
                        }
                    };

					using(PayPalAPIAAInterfaceClient client = new PayPalAPIAAInterfaceClient())
					{
						SetExpressCheckoutResponseType resp = client.SetExpressCheckout(ref requesterCredentials, request);
						PayPalUtility.HandleError(resp);

						Response.Redirect(string.Format("{0}?cmd=_express-checkout&token={1}",
						                                PayPalUtility.URL, resp.Token ?? resp.Any.InnerText));
					}
				}
				#endregion

				#region CreditCard
				if(userModel.PaymentType == "CreditCard")
				{
					Customer newCustomer = CreateGuestCustomer(userModel);
					string message;

					if(ProcessCrediCard(userModel, out message))
					{
						var isSuccess = ActivateAccount(newCustomer.CustomerID);

						try
						{
							MessageManager.SendCustomerWelcomeMessageNew(newCustomer, userModel.Firstname, 0);
							MessageManager.SendStoreOwnerRegistrationNotification(newCustomer, "", "", 0);
							SendAccountCreatedNotification(newCustomer, "creditcard");
						}
						catch(Exception ex)
						{
							Edrive.Helpers.Log.Event("Error", ex.Message, "");
						}

						if(isSuccess)
							return RedirectToAction("Success");
						else
							return View("MembershipRenewed");
					}
					else
					{
						ViewData["Msg"] = string.Format("Transaction Failed! {0}", message);
						return View();
					}
				}
				#endregion

				#region PredefinedForm
				if(userModel.PaymentType == "PreDefinedForm")
				{
					Customer newCustomer = CreateGuestCustomer(userModel);

					try
					{
						using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
						{
							//Update recoard from Guest to Customer
							var cust = entity.Customer.First(m => m.CustomerID == newCustomer.CustomerID);
							cust.Deleted = false;
							cust.ExpirationDate = DateTime.Now.AddYears(1);
							cust.IsTrial = true;
							cust.RegisterationDate = DateTime.Now;

							entity.SaveChanges();

							try
							{
								MessageManager.SendFreeAccessCustomerWelcomeMessageNew(cust, cust.Name, 0);
								MessageManager.SendStoreOwnerRegistrationNotification(cust, "", "", 0);
								SendAccountCreatedNotification(cust);
							}
							catch
							{
								//NOTE: it should normally send on the sever. Locally it doesn't send.
							}
							//MessageManager.SendStoreOwnerRegistrationNotification(cust, "", "", 0);
						}
						return RedirectToAction("Success");
					}
					catch(Exception ex)
					{
						ViewData["Msg"] = "Error" + ex.Message;
						return View("MembershipRenewed");
					}
				}
				#endregion
			}

			ViewData["StateProvince"] = GetStates();

			return View();
		}

		public ActionResult ConfirmMembership()
		{
			int userID = Convert.ToInt32(Request.Params["uid"]);
			string token = Request.Params["token"];
			string payerID = Request.Params["PayerID"];

			if(User.Identity.IsAuthenticated || String.IsNullOrWhiteSpace(token)
				|| userID <= 0 || String.IsNullOrWhiteSpace(payerID))
				return RedirectToAction("Index", "Home");

			using(eDriveAutoWebEntities entities = new eDriveAutoWebEntities())
			{
				var userDetails = entities.Customer.FirstOrDefault(c => c.CustomerID == userID);

				ViewBag.Token = token;
				ViewBag.PayerID = payerID;

				return View(userDetails);
			}
		}

		[HttpPost]
		public ActionResult ConfirmMembership(FormCollection collection)
		{
			if(User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			string submitType = collection["paymentSubmitType"].Trim().ToLower();
			int userID = Convert.ToInt32(collection["CustomerID"] ?? "0");
			string token = Request.Params["token"];
			string payerID = Request.Params["PayerID"];

			if(User.Identity.IsAuthenticated || String.IsNullOrWhiteSpace(token) 
				|| userID <= 0 || String.IsNullOrWhiteSpace(payerID))
				return RedirectToAction("Index", "Home");

			if(submitType == "confirm")
			{
				PayPalGetCustomerDetails(token);
				var result = PayPalDoExpressCheckout(token, payerID);
				ViewBag.Message = "failure";

				if(result == AckCodeType.Success)
				{
					if(ActivateAccount(userID))
					{
						ViewBag.Message = "success";
						
						using(eDriveAutoWebEntities entities = new eDriveAutoWebEntities())
						{
							var userDetails = entities.Customer.FirstOrDefault(c => c.CustomerID == userID);
							SendAccountCreatedNotification(userDetails, "paypal");
						}

						return RedirectToAction("Success");
					}
				}
			}
			else
			{
				return RedirectToAction("RegisterUser");
			}
			
			return View((Customer)null);
		}

        /// <summary>
        /// Retrieves the customer information related to supplied token
        /// for display and final transaction approval
        /// </summary>
        /// <returns></returns>
        protected void PayPalGetCustomerDetails(string token)
        {
            var requesterCredentials = PayPalUtility.BuildPayPalWebservice();
			
			// build getdetails request               
            GetExpressCheckoutDetailsReq req = new GetExpressCheckoutDetailsReq()                
            {                    
                GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType()                    
                {                        
                    Version = PayPalUtility.Version,                        
                    Token = token                    
                }                
            };                 
            
            // query PayPal for transaction details      
			using(PayPalAPIAAInterfaceClient client = new PayPalAPIAAInterfaceClient())
			{
				GetExpressCheckoutDetailsResponseType resp = client.GetExpressCheckoutDetails(ref requesterCredentials, req);
				PayPalUtility.HandleError(resp);
				GetExpressCheckoutDetailsResponseDetailsType respDetails = resp.GetExpressCheckoutDetailsResponseDetails;

				// setup UI and save transaction details to session                
				Session["CheckoutDetails"] = resp;
			}
        }

        /// <summary>
        /// Submits the transactions
        /// </summary>
        /// <returns></returns>
		protected AckCodeType PayPalDoExpressCheckout(string token, string payerID)
        {
            var requesterCredentials = PayPalUtility.BuildPayPalWebservice();

            // get transaction details            
            GetExpressCheckoutDetailsResponseType resp = Session["CheckoutDetails"] as GetExpressCheckoutDetailsResponseType;                     
            
            // prepare for commiting transaction            
            DoExpressCheckoutPaymentReq payReq = new DoExpressCheckoutPaymentReq()            
            {                
                DoExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType()                
                {                    
                    Version = PayPalUtility.Version,                   
                    DoExpressCheckoutPaymentRequestDetails 
                        = new DoExpressCheckoutPaymentRequestDetailsType()                    
                    {                        
                        Token = token,                        
                        PaymentAction = PaymentActionCodeType.Sale,                        
                        PaymentActionSpecified = true,                        
                        PayerID = payerID,                        
                        PaymentDetails = new PaymentDetailsType[] 
                        {                            
                            new PaymentDetailsType()                            
                            {                                
                                OrderTotal = new BasicAmountType()                                
                                {                                    
                                    currencyID = CurrencyCodeType.USD,                                    
                                    Value = "9.95"                                
                                }                            
                            }                        
                        }                    
                    }                
                }            
            };

			using(PayPalAPIAAInterfaceClient client = new PayPalAPIAAInterfaceClient())
			{
				// commit transaction and display results to user            
				DoExpressCheckoutPaymentResponseType doResponse = client.DoExpressCheckoutPayment(ref requesterCredentials, payReq);
				PayPalUtility.HandleError(resp);

				return doResponse.Ack;
			}
        }

		/// <summary>
		/// This methos proecess the Credit Card payments
		/// </summary>
		/// <param name="userModel"></param>
		/// <param name="strError"></param>
		/// <returns></returns>
		protected Boolean ProcessCrediCard(_UserRegisteration userModel, out string strError)
		{
			strError = "";
			// By default, this sample code is designed to post to our test server for
			// developer accounts: https://uat.payleap.com/transactservices.svc/ProcessCreditCard?
			// for real accounts (even in test mode), please make sure that you are
			// posting to: https://secure1.payleap.com/transactservices.svc/ProcessCreditCard?

			// Additional fields can be added here as outlined in the AIM integration
			// guide at: http://developers.payleap.com/developer/guides/transactionapi.pdf

			const string postUrl = "https://secure1.payleap.com/transactservices.svc/ProcessCreditCard";

			Dictionary<string, string> postValues = new Dictionary<string, string>();
			//the API Login ID and Transaction Key must be replaced with valid values
			String merchantLoginID = ConfigurationManager.AppSettings["PayLeapId"].ToString();
			String merchantTransKey = ConfigurationManager.AppSettings["PayLeapTransKey"].ToString();

			postValues.Add("UserName", merchantLoginID);
			postValues.Add("Password", merchantTransKey);
			postValues.Add("TransType", "Sale");
			postValues.Add("CardNum", userModel.CreditCardNumber);
			postValues.Add("ExpDate", string.Format("{0}{1}", userModel.ExpMonth, userModel.ExpYear));
			postValues.Add("MagData", "");
			postValues.Add("NameOnCard", string.Format("{0}{1}{2}", userModel.Firstname, ' ', userModel.Lastname));
			postValues.Add("Amount", "9.95");
			postValues.Add("InvNum", "");
			postValues.Add("PNRef", "");
			postValues.Add("Zip", (userModel.Zip.ToString().Length < 5 ? "0" : "") + userModel.Zip.ToString());
			postValues.Add("Street", userModel.Address);
			postValues.Add("CVNum", userModel.CVV);
			postValues.Add("ExtData", "");

			String postString = String.Empty;

			foreach(KeyValuePair<string, string> postValue in postValues)
			{
				postString += postValue.Key + "=" + HttpUtility.UrlEncode(postValue.Value) + "&";
			}

			postString = postString.TrimEnd('&');

			byte[] Params = Encoding.ASCII.GetBytes(postString);
			HttpWebRequest xmlHttp = (HttpWebRequest)WebRequest.Create(postUrl);
			xmlHttp.Method = "POST";
			xmlHttp.ContentType = "application/x-www-form-urlencoded";
			xmlHttp.ContentLength = Params.Length;

			using(Stream postData = xmlHttp.GetRequestStream())
			{
				postData.Write(Params, 0, Params.Length);
			}

			String sXMLResponse;
			using(HttpWebResponse xmlResponse = (HttpWebResponse)xmlHttp.GetResponse())
			using(Stream responseStream = xmlResponse.GetResponseStream())
			using(StreamReader streamReader = new StreamReader(responseStream))
			{
				sXMLResponse = streamReader.ReadToEnd();
			}

			PayLeapResponse pResponse = new PayLeapResponse();
			pResponse.Fill(sXMLResponse);

			if(pResponse.Result != 0)
			{
				Edrive.Helpers.Log.Event("Transaction Declined", pResponse.Response(), "");
			}
			else
			{
				Edrive.Helpers.Log.Event("Transaction Approved", pResponse.Response(), "");
			}

			return (pResponse.Result == 0);
		}

		/// <summary>
		/// To create the as Guest Customer which is not active
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns></returns>
		private Customer CreateGuestCustomer(_UserRegisteration userModel)
		{
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				#region CustomerCreate
				//Create user as Guest
				string ipaddress = Request.UserHostAddress;
				// if customer already exist as Guest-- then update the Guest--
				Customer newCustomer;

				if((newCustomer = entity.Customer.FirstOrDefault(m => m.Customer_Type.RoleName == "Guest" && m.Email == userModel.Email)) == null)
				{
					var custType = entity.Customer_Type.First(m => m.RoleName == "Guest").id;

					newCustomer = new Customer
					{
						Active = false,
						CustomerType = custType,
						Deleted = true,
						Email = userModel.Email,
						ExpirationDate = DateTime.Now,
						FirstName = userModel.Firstname,
						GUID = Guid.NewGuid().ToString(),
						IPAddress = ipaddress,
						IsNewsLetter = false,
						IsTrial = true,
						LastName = userModel.Lastname,
						Name = userModel.Firstname + " " + userModel.Lastname,
						Password = userModel.Password,
						Phone = String.Empty,
						RegisterationDate = DateTime.Now,
						zip = userModel.Zip,
						ADDRESS = userModel.Address,
						CITY = userModel.City,
						STATE = userModel.State
					};

					entity.Customer.AddObject(newCustomer);
					entity.SaveChanges();
				}
				else // update customer
				{
					newCustomer.Active = false;
					newCustomer.Deleted = true;
					newCustomer.Email = userModel.Email;
					newCustomer.ExpirationDate = DateTime.Now;
					newCustomer.FirstName = userModel.Firstname;
					newCustomer.GUID = Guid.NewGuid().ToString();
					newCustomer.IPAddress = ipaddress;
					newCustomer.IsNewsLetter = false;
					newCustomer.IsTrial = true;
					newCustomer.LastName = userModel.Lastname;
					newCustomer.Name = string.Format("{0} {1}", userModel.Firstname, userModel.Lastname);
					newCustomer.Password = userModel.Password;
					newCustomer.Phone = String.Empty;
					newCustomer.RegisterationDate = DateTime.Now;
					newCustomer.zip = userModel.Zip;
					newCustomer.ADDRESS = userModel.Address;
					newCustomer.CITY = userModel.City;
					newCustomer.STATE = userModel.State;

					entity.SaveChanges();
				}

				#endregion
				return newCustomer;
			}
		}

		private static List<SelectListItem> GetOptions()
		{
			var lstOptions = new List<SelectListItem>
			                 	{
			                 		new SelectListItem {Text = "360Jacksonville.com", Value = "360Jacksonville.com"},
			                 		new SelectListItem {Text = "SouthFlorida365.com", Value = "SouthFlorida365.com"},
			                 		new SelectListItem {Text = "MotoSeller.com"		, Value = "MotoSeller.com"},
			                 		new SelectListItem {Text = "Military.com"		, Value = "Military.com"},
			                 		new SelectListItem {Text = "Automotive Digest"	, Value = "Automotive Digest"},
			                 		new SelectListItem {Text = "Facebook"			, Value = "Facebook"},
			                 		new SelectListItem {Text = "Living Social"		, Value = "Living Social"},
			                 		new SelectListItem {Text = "Groupon"			, Value = "Groupon"},
			                 		new SelectListItem {Text = "Savings.com"		, Value = "Savings.com"},
			                 		new SelectListItem {Text = "Other"				, Value = "Other"}
			                 	};

			return lstOptions;
		}

		private void SendAccountCreatedNotification(Customer customer, string paymentType = "free")
		{
			string emailBody = String.Format(@"Hi Derek, <br/> New user has been registered, 'Limited access account', on www.edriveautos.com <br/> user email is: {0}", customer.Email);

			if(paymentType == "creditcard")
				emailBody = String.Format(@"Hi Derek, <br/> New user has been created ,'Full access account', on www.edriveautos.com 
											<br/> user email is: {0} <br/> user Payment Type: {1}", customer.Email, "Credit Card");
			if(paymentType == "paypal")
				emailBody = String.Format(@"Hi Derek, <br/> New user has been created ,'Full access account', on www.edriveautos.com 
											<br/> user email is: {0} <br/> user Payment Type: {1}", customer.Email, "PayPal");

			ThreadPool.QueueUserWorkItem(delegate
			{
				var smtpClient = new SmtpClient();

				var message = new MailMessage
				{
					From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"]),
					IsBodyHtml = true,
					Body = emailBody,
					BodyEncoding = Encoding.UTF8
				};
				message.To.Add(new MailAddress(ConfigurationManager.AppSettings["EmailTO"]));

				try
				{
					smtpClient.Send(message);
				}
				catch
				{
					//LOG
				}
			});
		}

		private bool ActivateAccount(int id)
		{
			bool success = true;

			try
			{
				using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
				{
					//Update recoard from Guest to Customer
					var custType = entity.Customer_Type.First(m => m.RoleName == "Customer").id;
					var cust = entity.Customer.First(m => m.CustomerID == id);

					cust.Active = true;
					cust.CustomerType = custType;
					cust.Deleted = false;
					cust.ExpirationDate = DateTime.Now.AddDays(11).AddHours(-1);
					cust.IsTrial = false;
					cust.RegisterationDate = DateTime.Now;

					entity.SaveChanges();

					try
					{
						MessageManager.SendCustomerWelcomeMessageNew(cust, cust.Name, 0);
						MessageManager.SendStoreOwnerRegistrationNotification(cust, "", "", 0);
					}
					catch
					{
						//LOG: Error occurred while sending an email
					}
				}
			}
			catch(Exception ex)
			{
				ViewData["Msg"] = string.Format("Error: {0}", ex.Message);
				success = false;
			}

			return success;
		}

		private List<SelectListItem> GetStates()
		{
			var states = _stateProvinceService.GetStatesByCountryCode("USA")
				.OrderBy(c => c.Name).Select(c => new SelectListItem
				                                  	{
				                                  		Text = c.Abbreviation,
				                                  		Value = c.StateProvinceID.ToString()
				                                  	}).ToList();

			return states;
		}
	}
}
