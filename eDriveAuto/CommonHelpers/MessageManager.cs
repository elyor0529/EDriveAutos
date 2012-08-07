using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;
using Customer = Edrive.Models.Customer;
using MessageTemplateLocalized = Edrive.Models.MessageTemplateLocalized;

//using Edrive.Edrivie_Service_Ref;

namespace Edrive.CommonHelpers
{
    public static class MessageManager
    {
        /// <summary>
        /// to return the list of Tokens
        /// </summary>
        /// <returns></returns>
        public static string[] GetListOfAllowedTokens()
        {
            var allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%Order.OrderNumber%");
            allowedTokens.Add("%Order.CustomerFullName%");
            allowedTokens.Add("%Order.CustomerEmail%");
            allowedTokens.Add("%Order.BillingFirstName%");
            allowedTokens.Add("%Order.BillingLastName%");
            allowedTokens.Add("%Order.BillingPhoneNumber%");
            allowedTokens.Add("%Order.BillingEmail%");
            allowedTokens.Add("%Order.BillingFaxNumber%");
            allowedTokens.Add("%Order.BillingCompany%");
            allowedTokens.Add("%Order.BillingAddress1%");
            allowedTokens.Add("%Order.BillingAddress2%");
            allowedTokens.Add("%Order.BillingCity%");
            allowedTokens.Add("%Order.BillingStateProvince%");
            allowedTokens.Add("%Order.BillingZipPostalCode%");
            allowedTokens.Add("%Order.BillingCountry%");
            allowedTokens.Add("%Order.ShippingMethod%");
            allowedTokens.Add("%Order.ShippingFirstName%");
            allowedTokens.Add("%Order.ShippingLastName%");
            allowedTokens.Add("%Order.ShippingPhoneNumber%");
            allowedTokens.Add("%Order.ShippingEmail%");
            allowedTokens.Add("%Order.ShippingFaxNumber%");
            allowedTokens.Add("%Order.ShippingCompany%");
            allowedTokens.Add("%Order.ShippingAddress1%");
            allowedTokens.Add("%Order.ShippingAddress2%");
            allowedTokens.Add("%Order.ShippingCity%");
            allowedTokens.Add("%Order.ShippingStateProvince%");
            allowedTokens.Add("%Order.ShippingZipPostalCode%");
            allowedTokens.Add("%Order.ShippingCountry%");
            allowedTokens.Add("%Order.TrackingNumber%");
            allowedTokens.Add("%Order.Product(s)%");
            allowedTokens.Add("%Order.CreatedOn%");
            allowedTokens.Add("%Order.OrderURLForCustomer%");
            allowedTokens.Add("%Customer.Email%");
            allowedTokens.Add("%Customer.Username%");
            allowedTokens.Add("%Customer.PasswordRecoveryURL%");
            allowedTokens.Add("%Customer.AccountActivationURL%");
            allowedTokens.Add("%Customer.FullName%");
            allowedTokens.Add("%Product.Name%");
            allowedTokens.Add("%Product.ShortDescription%");
            allowedTokens.Add("%Product.ProductURLForCustomer%");
            allowedTokens.Add("%ProductVariant.FullProductName%");
            allowedTokens.Add("%ProductVariant.StockQuantity%");
            allowedTokens.Add("%NewsComment.NewsTitle%");
            allowedTokens.Add("%BlogComment.BlogPostTitle%");
            allowedTokens.Add("%NewsLetterSubscription.Email%");
            allowedTokens.Add("%NewsLetterSubscription.ActivationUrl%");
            allowedTokens.Add("%NewsLetterSubscription.DeactivationUrl%");
            allowedTokens.Add("%GiftCard.SenderName%");
            allowedTokens.Add("%GiftCard.SenderEmail%");
            allowedTokens.Add("%GiftCard.RecipientName%");
            allowedTokens.Add("%GiftCard.RecipientEmail%");
            allowedTokens.Add("%GiftCard.Amount%");
            allowedTokens.Add("%GiftCard.CouponCode%");
            allowedTokens.Add("%GiftCard.Message%");
            allowedTokens.Add("%InquiryPerson.Phone%");
            allowedTokens.Add("%InquiryPerson.Zip%");
            allowedTokens.Add("%InquiryPerson.Contact%");
            allowedTokens.Add("%InquiryPerson.Comments%");
            allowedTokens.Add("%InquiryPerson.Interested%");
            allowedTokens.Add("%InquiryPerson.FirstName%");
            allowedTokens.Add("%InquiryPerson.LastName%");
            allowedTokens.Add("%InquiryPerson.TradeIn%");
            allowedTokens.Add("%Partners.FirstName%");
            allowedTokens.Add("%Partners.LastName%");
            allowedTokens.Add("%Partners.Phone%");
            allowedTokens.Add("%Partners.Email%");
            allowedTokens.Add("%Partners.Company%");
            allowedTokens.Add("%Partners.Website%");
            allowedTokens.Add("%Partners.Comments%");
            allowedTokens.Add("%Dealers.Name%");
            allowedTokens.Add("%Dealers.Title%");
            allowedTokens.Add("%Dealers.DealershipName%");
            allowedTokens.Add("%Dealers.DealershipAddress%");
            allowedTokens.Add("%Dealers.City%");
            allowedTokens.Add("%Dealers.Country%");
            allowedTokens.Add("%Dealers.State%");
            allowedTokens.Add("%Dealers.Zip%");
            allowedTokens.Add("%Dealers.Telephone%");
            allowedTokens.Add("%Dealers.Fax%");
            allowedTokens.Add("%Dealers.Email%");
            allowedTokens.Add("%Dealers.HearAbout%");
            allowedTokens.Add("%Dealers.Notes%");

            return allowedTokens.ToArray();
        }
        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>
        public static string[] GetListOfCampaignAllowedTokens()
        {
            var allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%NewsLetterSubscription.Email%");
            allowedTokens.Add("%NewsLetterSubscription.ActivationUrl%");
            allowedTokens.Add("%NewsLetterSubscription.DeactivationUrl%");
            return allowedTokens.ToArray();
        }
        /// <summary>
        /// to  Get Localized MessageTemplate for each section
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static MessageTemplateLocalized GetLocalizedMessageTemplate(String templateName,Int32 langid)
        {
            using (eDriveAutoWebEntities _edriveEntitye = new eDriveAutoWebEntities())
            {
                return _edriveEntitye.MessageTemplateLocalized.FirstOrDefault(m => m.MessageTemplate.Name== templateName);
            }
 
        }
        /// <summary>
        /// to  Get Localized MessageTemplate Details for Message Template if not exist it will create the new template
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static MessageTemplateLocalized GetLocalizedMessageTemplate_by_templateID(Int32 templateid)
        {
            using (eDriveAutoWebEntities _edriveEntitye = new eDriveAutoWebEntities())
            {
                var localizedTemp= _edriveEntitye.MessageTemplateLocalized.FirstOrDefault(m => m.MessageTemplateID == templateid);
                if (localizedTemp == null)
                {
                    localizedTemp = new MessageTemplateLocalized { MessageTemplateID=templateid};
                    _edriveEntitye.MessageTemplateLocalized.AddObject(localizedTemp);
                    _edriveEntitye.SaveChanges();

                }
                return localizedTemp;

            }

        }
       /// <summary>
       /// Administarator Displaly Name
       /// </summary>
        public static string AdminEmailDisplayName
        {
            get
            {
                using (eDriveAutoWebEntities _edriveEntitye = new eDriveAutoWebEntities())
                {
                    return  (_edriveEntitye.Settings.First(m => m.Name == "Email.AdminEmailDisplayName").Value);
                }
            }
            
        }
        /// <summary>
        /// to return the Admin Email Address
        /// </summary>
        public static
            String AdminEmailAddress
        { 
            get
            {

                using (eDriveAutoWebEntities _edriveEntitye = new eDriveAutoWebEntities())
                {

                    return ( _edriveEntitye.Settings.First(m => m.Name == "Email.AdminEmailAddress").Value);
                }
            }
            
        }
      /// <summary>
      /// To Send customer Welcome Message 
      /// </summary>
      /// <param name="customer"></param>
      /// <param name="languageId"></param>
        public static void SendCustomerWelcomeMessage(Edrivie_Service_Ref.Customer customer, int languageId)
        {
            string templateName = "Customer.WelcomeMessage";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BCCEmailAddresses;
            string cc = AdminEmailAddress;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.email,customer.Name);
            InsertQueuedEmail( from, to, cc, bcc, subject, body);
            //return queuedEmail.QueuedEmailId;

        }
        /// <summary>
        /// To send the Enquir to Dealer
        /// </summary>
        /// <param name="strEnquiry"></param>
        /// <param name="LanguageID"></param>
        public static void SendEnquiryToDealer(string[] strEnquiry, int LanguageID)
        {
            {
                string TemplateName = "EDrive.EmailTheSeller";
                var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
                string subject = ReplaceMessageTemplateTokens(strEnquiry, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokens(strEnquiry, localizedMessageTemplate.Body);
                string cc = AdminEmailAddress;
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                //var from = new MailAddress(strEnquiry[0], strEnquiry[0]);
                var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
                
                var to = new MailAddress(strEnquiry[1], strEnquiry[1]);
                InsertQueuedEmail(from, to, cc, bcc, subject, body);
               
            }
        }
        public static string ReplaceMessageTemplateTokens(string[] strEnquiry, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("InquiryPerson.ClientEmail", HttpUtility.HtmlEncode(strEnquiry[0]));
            tokens.Add("Product.Name", HttpUtility.HtmlEncode(strEnquiry[2]));
            tokens.Add("InquiryPerson.Phone", strEnquiry[3]);
            tokens.Add("InquiryPerson.Zip", HttpUtility.HtmlEncode(strEnquiry[4]));
            tokens.Add("InquiryPerson.Contact", HttpUtility.HtmlEncode(strEnquiry[5]));
            tokens.Add("InquiryPerson.Comments", HttpUtility.HtmlEncode(strEnquiry[6]));
            tokens.Add("InquiryPerson.Interested", HttpUtility.HtmlEncode(strEnquiry[7]));
            tokens.Add("InquiryPerson.FirstName", HttpUtility.HtmlEncode(strEnquiry[8]));
            tokens.Add("InquiryPerson.LastName", HttpUtility.HtmlEncode(strEnquiry[9]));
            tokens.Add("InquiryPerson.TradeIn", HttpUtility.HtmlEncode(strEnquiry[10]));
            tokens.Add("InquiryPerson.DealerName", HttpUtility.HtmlEncode(strEnquiry[11]));
            tokens.Add("InquiryPerson.VIN", HttpUtility.HtmlEncode(strEnquiry[12]));
            tokens.Add("InquiryPerson.IsDealer", HttpUtility.HtmlEncode(strEnquiry[13]));

            foreach (string token in tokens.Keys)
                template = template.Replace(string.Format(@"%{0}%", token), tokens[token]);
            return template;
        }
        public static string ReplaceMessageTemplateTokens(Edrive.Edrivie_Service_Ref.Customer customer, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("Customer.Name", customer.FirstName);
            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.email));
            tokens.Add("Customer.Username", HttpUtility.HtmlEncode(customer.email));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.Name));
            tokens.Add("Customer.Password", HttpUtility.HtmlEncode(customer.password ));

            string passwordRecoveryUrl = string.Empty;

            //create guid
            if (String.IsNullOrEmpty(customer.customerGUID))
            {
                using(Edrivie_Service_Ref.Edrive_ServiceClient _servicae=new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    customer.customerGUID = _servicae.GetDealerGUID(customer.customerID);

                }
            }
            passwordRecoveryUrl = string.Format("{0}Account/Dealer/ChangePassword/{1}/{2}", SettingManager.StoreUrl, customer.email, customer.customerGUID);
            tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryUrl);

            string accountActivationUrl = string.Empty;
          //  accountActivationUrl = string.Format("{0}accountactivation.aspx?act={1}&email={2}", SettingManager.StoreUrl, customer.AccountActivationToken, customer.Email);
            tokens.Add("Customer.AccountActivationURL", accountActivationUrl);

            foreach (string token in tokens.Keys)
            {
                template = template.Replace(String.Format("%{0}%", token), tokens[token]);
            }

            return template;
        }

        public static string ReplaceMessageTemplateTokens(Edrive.Models.Customer customer, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("Customer.Name", customer.FirstName);
            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.Username", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.Name));
            tokens.Add("Customer.Password", HttpUtility.HtmlEncode(customer.Password));

            string passwordRecoveryUrl = string.Empty;
            // create customer guid

            if (String.IsNullOrEmpty(customer.GUID))
            {
                using (eDriveAutoWebEntities _entitye = new eDriveAutoWebEntities())
                {
                    var cust = _entitye.Customer.FirstOrDefault(m => m.CustomerID == customer.CustomerID);
                    customer.GUID = cust.GUID = Guid.NewGuid().ToString();
                    _entitye.SaveChanges();

                }


            }
            passwordRecoveryUrl = string.Format("{0}Account/Customer/ChangePassword/{1}/{2}", SettingManager.StoreUrl,  customer.Email,customer.GUID);
            tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryUrl);
           

            string accountActivationUrl = string.Empty;
            //  accountActivationUrl = string.Format("{0}accountactivation.aspx?act={1}&email={2}", SettingManager.StoreUrl, customer.AccountActivationToken, customer.Email);
            tokens.Add("Customer.AccountActivationURL", accountActivationUrl);

            foreach (string token in tokens.Keys)
            {
                template = template.Replace(String.Format("%{0}%", token), tokens[token]);
            }

            return template;
        }
                /// <summary>
 /// to send the Messages
 /// </summary>
 /// <param name="from"></param>
 /// <param name="to"></param>
 /// <param name="cc"></param>
 /// <param name="bcc"></param>
 /// <param name="subject"></param>
 /// <param name="body"></param>
         public  static void InsertQueuedEmail(MailAddress from, MailAddress to, string cc, string bcc, string subject, string body,Int32 priority=3)
        {
            try
            {
             
             ////----------insert mail records-------------
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                Nop_QueuedEmail newobj=new Nop_QueuedEmail {Bcc=bcc,Body=body,Cc=cc,CreatedOn=DateTime.Now,From=from.Address,FromName=from.DisplayName,Priority=priority,SentOn=DateTime.Now,Subject=subject,To=to.Address,ToName=to.DisplayName};
                _entity.Nop_QueuedEmail.AddObject(newobj);
                _entity.SaveChanges();
            }

            //var to = new MailAddress(to., strEnquiry[0]);//uncomment this line for live site
              // to = new MailAddress("rtest1@sarnatechnologies.com");//this line for development
              //cc = System.Configuration.ConfigurationManager.AppSettings["CC"].ToString();
            System.Net.Mail.SmtpClient obj = new System.Net.Mail.SmtpClient();
            System.Net.Mail.MailMessage Mailmsg = new System.Net.Mail.MailMessage();
            Mailmsg.To.Clear();

            Mailmsg.To.Add(to);
            Mailmsg.From = (from);
            //Mailmsg.CC.Add(cc);
            Mailmsg.Subject = subject;

            Mailmsg.Body = body;
            Mailmsg.IsBodyHtml = true;


            obj.Send(Mailmsg);
              }
            catch (Exception ex)
            {

                throw ex;
            }
             // var queuedEmail = InsertQueuedEmail(5, from, to, cc, bcc, subject, body, DateTime.Now, 0, null);

             
            
        }
        /// <summary>
        /// To send Email 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void SendEmail(string subject, string body, MailAddress from, MailAddress to)
        {
            System.Net.Mail.SmtpClient obj = new System.Net.Mail.SmtpClient();
            System.Net.Mail.MailMessage Mailmsg = new System.Net.Mail.MailMessage();
            Mailmsg.To.Clear();

            Mailmsg.To.Add(to);//------uncomment for live site deployment
            //to = new MailAddress("rtest1@sarnatechnologies.com");//this line for development
            #region for testing 
            //------delete this region after testing, no need for cc
            //var cc = System.Configuration.ConfigurationManager.AppSettings["CC"].ToString();
            //Mailmsg.CC.Add(cc);
            #endregion

            Mailmsg.From = (from);
            Mailmsg.Subject = subject;

            Mailmsg.Body = body;
            Mailmsg.IsBodyHtml = true;
            obj.Send(Mailmsg);
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(String Msg,
            string template)
        {
            var tokens = new NameValueCollection();
            //tokens.Add("%Store.Name%" );
            //tokens.Add("%Store.URL%");
            //tokens.Add("%Store.Email%");

           // tokens.Add("BlogComment.BlogPostTitle", HttpUtility.HtmlEncode(blogComment.BlogPost.BlogPostTitle));

            foreach (string token in tokens.Keys)
            {
                template = template.Replace(String.Format(@"%{0}%", token), tokens[token]);
            }

            return template;
        }



        /// <summary>
        /// Replaces a message template tokens for Bottlemine Event Invite List
        /// </summary>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokensPartners(string[] strEnquiry, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);

            tokens.Add("Partners.FirstName", HttpUtility.HtmlEncode(strEnquiry[1]));
            tokens.Add("Partners.LastName", strEnquiry[2]);
            tokens.Add("Partners.Phone", HttpUtility.HtmlEncode(strEnquiry[3]));
            tokens.Add("Partners.Email", strEnquiry[0]);
            tokens.Add("Partners.Company", HttpUtility.HtmlEncode(strEnquiry[4]));
            tokens.Add("Partners.Website", HttpUtility.HtmlEncode(strEnquiry[5]));
            tokens.Add("Partners.Comments", HttpUtility.HtmlEncode(strEnquiry[6]));

            foreach (string token in tokens.Keys)
                template = template.Replace(string.Format(@"%{0}%", token), tokens[token]);
            return template;
        }
        /// <summary>
        /// Sends Email to Admin
        /// </summary>
        /// <returns>Queued email identifier</returns>
        public static void SendPartnersToAdmin(string[] strEnquiry, int LanguageID)
        {
            {
                string TemplateName = "EDrive.EmailForPartner";
                var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
                
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

                string subject = ReplaceMessageTemplateTokensPartners(strEnquiry, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokensPartners(strEnquiry, localizedMessageTemplate.Body);
                string cc = string.Empty;
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                var from = new MailAddress(strEnquiry[0], strEnquiry[0]);
                var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
                InsertQueuedEmail( from, to, cc, bcc, subject, body);
                
            }
        }
        /// <summary>
        /// Sends Email to Admin
        /// </summary>
        /// <returns>Queued email identifier</returns>
        public static void SendDealersToAdmin(string[] strEnquiry, int LanguageID)
        {
            {
                string TemplateName = "EDrive.BecomeADealer";
                var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
                 
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

                string adminEmailID = SettingManager.GetSettingValue("Email.AdminEmailUser"); ;

                string subject = ReplaceMessageTemplateTokensDealers(strEnquiry, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokensDealers(strEnquiry, localizedMessageTemplate.Body);
                string cc = string.Empty;
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                var from = new MailAddress(adminEmailID, adminEmailID);
                var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
                InsertQueuedEmail( from, to, cc, bcc, subject, body,  0);
            }
        }
        /// <summary>
        /// When User register for finance request this method send the mail regarding this details to admin
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="languageId"></param>
        public static void SendFinanceRequest(FinancingInfo UserRequestModel)
        {
            var adminEmailID = MessageManager.AdminEmailAddress;
            var from = new MailAddress(adminEmailID, adminEmailID);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            string cc = string.Empty;
            string bcc = "";// localizedMessageTemplate.BCCEmailAddresses;
            String subject = "New Finance Request";
            StringBuilder body = new StringBuilder ();
            body.Append(@"<div style='font-size:12px;font-weight:bold;font-family:arial;height:20px;'>New  Finance Request</div><table><thead><tr style='background: #0896EF;color: white;font-weight: bold;text-align: center;font-size: 12px;font-family: arial;' ><td>Name</td><td>Personal Details</td><td>Employement Details</td> 
<td>Income Details</td><td>Created on</td> </tr></thead><tbody>");
            body.Append("<tr><td style='font-size:12px;font-family:arial;border: solid 1px #AFDFFD;'><b>" + UserRequestModel.FirstName
                + " " + UserRequestModel.LastName + "</b></td><td style='font-size:12px;font-family:arial;border: solid 1px #AFDFFD;'><div style='text-align:right;margin-right:5px'>" + UserRequestModel.FirstName + " " + UserRequestModel.LastName + ",<br />"
            +UserRequestModel.StreedAddress+",<br />"+UserRequestModel.City+", "+UserRequestModel.State
            +",<br/><b>"+UserRequestModel.Email+", <br /></b><b>Home Phone:</b>"+
            UserRequestModel.HomePhone+"<br /><b>Mobile Phone:</b> "+UserRequestModel.MobilePhone
            +"<br /><b>Residence Type:</b> "+UserRequestModel.ResidenceType+"<br />Staying from " +
            UserRequestModel.HowLongAtAddressYear+" Years,"+
            UserRequestModel.HowLongAtAddressMonth+" ,Months <br /><b>Monthly Rent:</b> "+
            UserRequestModel.MonthlyRent + "</div></td><td style='font-size:12px;font-family:arial;border: solid 1px #AFDFFD;'> <div style='text-align:right;padding-right:5px;'>" +
            UserRequestModel.EmpolyerName + ",<br />" + UserRequestModel.JobTitle 
            + ",<br /> <b>Work Phone</b>" + UserRequestModel.WorkPhone + ", <br/> <b>Working form " +
            UserRequestModel.WorkedForYear + " Years " + UserRequestModel.WorkedForMonth +
            " Months </b> </div></td><td style='font-size:12px;font-family:arial;border: solid 1px #AFDFFD;'><div style='text-align:right;padding-right:5px;'> <b>Monthly Income: </b>" +
            UserRequestModel.MonthlyIncome + " <br /><b>Downpayment: </b> " + 
            UserRequestModel.DownPayment + " <br /><b>Is Bankrupt: </b>" + 
            (UserRequestModel.IsBankrupt == true ? "Yes" : "No") +
            "<br><b>Credit Score: </b>" + UserRequestModel.CreditScore + "<br/><b>Co-Signor Available: </b> " + (UserRequestModel.IsCoSigner == true ? "Yes" : "No") + " <br /> </div>" +
            "</td><td style='font-size:12px;font-family:arial;border: solid 1px #AFDFFD;'>" + UserRequestModel.CreatedOn.ToString("D") + "</td> </tr></tbody></table>");         
            InsertQueuedEmail(from, to, cc, bcc, subject, body.ToString(), 0);
        }

        /// <summary>
        /// Replaces a message template tokens for Bottlemine Event Invite List
        /// </summary>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokensDealers(string[] strEnquiry, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);

            tokens.Add("Dealers.FirstName", HttpUtility.HtmlEncode(strEnquiry[1]));
            tokens.Add("Dealers.LastName", HttpUtility.HtmlEncode(strEnquiry[2]));
            tokens.Add("Dealers.Title", strEnquiry[3]);
            tokens.Add("Dealers.DealershipName", HttpUtility.HtmlEncode(strEnquiry[4]));
         
            tokens.Add("Dealers.State", HttpUtility.HtmlEncode(strEnquiry[5]));
            tokens.Add("Dealers.Zip", HttpUtility.HtmlEncode(strEnquiry[7]));
            tokens.Add("Dealers.Telephone", HttpUtility.HtmlEncode(strEnquiry[6]));
         
            tokens.Add("Dealers.Email", strEnquiry[0]);

         

            foreach (string token in tokens.Keys)
                template = template.Replace(string.Format(@"%{0}%", token), tokens[token]);
            return template;
        }

        public static void SendAdminToDealers(string[] strEnquiry, int LanguageID)
        {
            {
                string TemplateName = "EDrive.EmailForDealer";
                var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
               
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

                string subject = ReplaceMessageTemplateTokensDealers(strEnquiry, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokensDealers(strEnquiry, localizedMessageTemplate.Body);
                string cc = AdminEmailAddress;
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
                var to = new MailAddress(strEnquiry[0], strEnquiry[0]);
               InsertQueuedEmail(from, to, cc, bcc, subject, body,  0);
               
            }
        }
          
      
        public static string ReplaceMessageTemplateTokens_for_newsletter(Edrive.Models.Customer subscription, string template)
        {
            var tokens = new NameValueCollection();

            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("NewsLetterSubscription.Email", HttpUtility.HtmlEncode(subscription.Email));
            tokens.Add("NewsLetterSubscription.ActivationUrl", String.Format("{0}newslettersubscriptionactivation.aspx?t={1}&active=1", SettingManager.StoreUrl, Guid.NewGuid()));
            tokens.Add("NewsLetterSubscription.DeactivationUrl", String.Format("{0}newslettersubscriptionactivation.aspx?t={1}&active=0", SettingManager.StoreUrl, Guid.NewGuid()));

             
            foreach (string token in tokens.Keys)
            {
                if(template.Contains("%"+token+"%"))
                template = template.Replace(token, tokens[token]);
            }

            return template;
        }
        public static void SendAdminToContact(string[] strEnquiry, int LanguageID)
        {
            {
                string TemplateName = "EDrive.EmailAdminTocontactUs";
                var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
                if (localizedMessageTemplate == null)
                    return ;

                string subject = ReplaceMessageTemplateTokensContactUs(strEnquiry, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokensContactUs(strEnquiry, localizedMessageTemplate.Body);
                string cc = AdminEmailAddress;
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                var from = new MailAddress(strEnquiry[0], strEnquiry[0]);
                var to = new MailAddress(AdminEmailAddress, AdminEmailAddress);
                InsertQueuedEmail(from, to, cc, bcc, subject, body,0);
              //  return queuedEmail.QueuedEmailId;
            }
        }
        public static string ReplaceMessageTemplateTokensContactUs(string[] strEnquiry, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Contact.Email", strEnquiry[0]);
            tokens.Add("Contact.Name", HttpUtility.HtmlEncode(strEnquiry[1]));
            tokens.Add("Contact.Address", HttpUtility.HtmlEncode(strEnquiry[2]));
            tokens.Add("Contact.City", HttpUtility.HtmlEncode(strEnquiry[3]));
            tokens.Add("Contact.Country", HttpUtility.HtmlEncode(strEnquiry[4]));
            tokens.Add("Contact.State", HttpUtility.HtmlEncode(strEnquiry[5]));
            tokens.Add("Contact.Zip", HttpUtility.HtmlEncode(strEnquiry[6]));
            tokens.Add("Contact.Telephone", HttpUtility.HtmlEncode(strEnquiry[7]));
            tokens.Add("Contact.Fax", HttpUtility.HtmlEncode(strEnquiry[8]));
            tokens.Add("Contact.HearAbout", HttpUtility.HtmlEncode(strEnquiry[9]));
            tokens.Add("Contact.Type", strEnquiry[10]);

            foreach (string token in tokens.Keys)
                template = template.Replace(string.Format(@"%{0}%", token), tokens[token]);
            return template;
        }
        /// <summary>
        /// Send Message when User register first time
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="firstName"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public static void SendCustomerWelcomeMessageNew(Customer customer, string firstName, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
			
            string templateName = "Customer.WelcomeMessage";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
          
            string subject = ReplaceMessageTemplateTokensNew(customer, firstName, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokensNew(customer, firstName, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BCCEmailAddresses;
            string cc = AdminEmailAddress;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.Name);
            InsertQueuedEmail( from, to, cc, bcc, subject, body,  0);
        }

		public static void SendFreeAccessCustomerWelcomeMessageNew(Customer customer, string firstName, int languageId)
		{
			if(customer == null)
				throw new ArgumentNullException("customer");


			string templateName = "Customer.WelcomeMessagePreApp";
			var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);


			string subject = ReplaceMessageTemplateTokensNew(customer, firstName, localizedMessageTemplate.Subject);
			string body = ReplaceMessageTemplateTokensNew(customer, firstName, localizedMessageTemplate.Body);
			string bcc = localizedMessageTemplate.BCCEmailAddresses;
			string cc = AdminEmailAddress;
			var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
			var to = new MailAddress(customer.Email, customer.Name);
			InsertQueuedEmail(from, to, cc, bcc, subject, body, 0);

		}

        public static string ReplaceMessageTemplateTokensNew(Customer customer, string firstName, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("Customer.Name", firstName);
            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.Username", customer.Email);
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.Name));
            tokens.Add("Customer.Password", HttpUtility.HtmlEncode(customer.Password));

            //string passwordRecoveryUrl = string.Empty;
            //passwordRecoveryUrl = string.Format("{0}passwordrecovery.aspx?prt={1}&email={2}", SettingManager.StoreUrl, customer.PasswordRecoveryToken, customer.Email);
            //tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryUrl);

            //string accountActivationUrl = string.Empty;
            //accountActivationUrl = string.Format("{0}accountactivation.aspx?act={1}&email={2}", SettingManager.StoreUrl, customer.AccountActivationToken, customer.Email);
            //tokens.Add("Customer.AccountActivationURL", accountActivationUrl);

            foreach (string token in tokens.Keys)
            {
                if (template.Contains("%" + token + "%"))
                    template = template.Replace("%" + token + "%", tokens[token]);
            }

            return template;
        }

        public static void SendStoreOwnerRegistrationNotification(Customer customer, string code, string here, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");


            var adminEmailID = MessageManager.AdminEmailAddress;

            string templateName = "CustomerRegistered.StoreOwnerNotification";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);

            string subject = ReplaceMessageTemplateTokensForAdmin(customer, code, here, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokensForAdmin(customer, code, here, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BCCEmailAddresses;
            string cc = string.Empty;
            var from = new MailAddress(adminEmailID, adminEmailID);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
         InsertQueuedEmail( from, to, cc, bcc, subject, body,  0);
          
        }

		public static void SendVehicleRequestToDealers(NoResultsSearchFilter vehicleDetails, List<Core.Model.Customer> dealers, int languageId)
		{
			const string templateName = "CustomerVehicleRequest.DealerNotification";
			var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
			string template = localizedMessageTemplate.Body;
			string make = "Any";
			string model = "Any";
			string body = "Any";
			string transmission = vehicleDetails.NoResultsTransmission != "-1" ? vehicleDetails.NoResultsTransmission : "Any";
			string engine = vehicleDetails.NoResultsEngine != "-1" ? vehicleDetails.NoResultsEngine : "Any";
			string driverType = vehicleDetails.NoResultsDriveType != "-1" ? vehicleDetails.NoResultsDriveType : "Any";
			string maximumMileage = vehicleDetails.NoResultsMileage.GetValueOrDefault(0) > 0 ? vehicleDetails.NoResultsMileage.ToString() : "Any";
			string yearFrom = vehicleDetails.NoResultsYearMin.GetValueOrDefault(0) > 0 ? vehicleDetails.NoResultsYearMin.ToString() : "Any";
			string yearTo = vehicleDetails.NoResultsYearMax.GetValueOrDefault(0) > 0 ? vehicleDetails.NoResultsYearMax.ToString() : "Any";
			string minPrice = vehicleDetails.NoResultsPriceMin.GetValueOrDefault(0) > 0 ? vehicleDetails.NoResultsPriceMin.ToString() : "Any";
			string maxPrice = vehicleDetails.NoResultsPriceMax.GetValueOrDefault(0) > 0 ? vehicleDetails.NoResultsPriceMax.ToString() : "Any";
			string firstName = String.Empty;
			string lastName = String.Empty;
			string phoneNo = String.Empty;
			string email = HttpContext.Current.User.Identity.Name.ToLower();
			int userId = 0;
			
			using(eDriveAutoWebEntities entities = new eDriveAutoWebEntities())
			{
				var user = entities.Customer.FirstOrDefault(c => c.Email.ToLower() == email);

				if(user != null)
				{
					userId = user.CustomerID;
					firstName = user.FirstName;
					lastName = user.LastName;
					email = user.Email;
					phoneNo = user.Phone;
				}
				else
				{
					return;//no such user found
				}
			}

			using (Edrive_ServiceClient client = new Edrive_ServiceClient())
			{
				if(vehicleDetails.NoResultsMake > 0)
					make = client.GetMakeById(vehicleDetails.NoResultsMake).make;
				if(vehicleDetails.NoResultsModel > 0)
					model = client.GetModelbyId(vehicleDetails.NoResultsModel).modelName;
				if(vehicleDetails.NoResultsBody > 0)
					body = client.GetBodybyId(vehicleDetails.NoResultsBody).body;
			}

			template = ReplaceMessageTemplateTokensForDealerNotification(template, make, model, maximumMileage, yearFrom, yearTo,
			                                                             minPrice, maxPrice, body, transmission, engine,
			                                                             driverType, firstName, lastName, phoneNo, email);

			SaveVehicleRequest(minPrice, yearTo, driverType, engine, transmission, maxPrice, maximumMileage, userId, yearFrom, body, model, make);
			SendEmailToDealers(localizedMessageTemplate, dealers, template);
		}

    	private static void SaveVehicleRequest(string minPrice, string yearTo, string driverType, string engine,
    	                                       string transmission, string maxPrice, string maximumMileage, int userId,
    	                                       string yearFrom, string body, string model, string make)
    	{
    		using (eDriveAutoWebEntities entities = new eDriveAutoWebEntities())
    		{
    			CustomerVehicleRequests vehicleRequests = new CustomerVehicleRequests
    			                                          	{
    			                                          		UserID = userId,
    			                                          		Body = body,
    			                                          		CreationDate = DateTime.Now,
    			                                          		Make = make,
    			                                          		Model = model,
    			                                          		MaximumMileage = maximumMileage,
    			                                          		YearFrom = yearFrom,
    			                                          		YearTo = yearTo,
    			                                          		MinPrice = minPrice,
    			                                          		MaxPrice = maxPrice,
    			                                          		Transmission = transmission,
    			                                          		Engine = engine,
    			                                          		DriverType = driverType
    			                                          	};

    			entities.CustomerVehicleRequests.AddObject(vehicleRequests);
    			entities.SaveChanges();
    		}
    	}

    	private static void SendEmailToDealers(MessageTemplateLocalized localizedMessageTemplate, IEnumerable<Core.Model.Customer> dealers, string template)
		{
			var adminEmailID = MessageManager.AdminEmailAddress;
			string bcc = localizedMessageTemplate.BCCEmailAddresses;
			string cc = ConfigurationManager.AppSettings["EmailTO"];
			var from = new MailAddress(adminEmailID, adminEmailID);

			foreach(var dealer in dealers)
			{
				string emailBody = template.Replace("%Dealer.Name%", string.Format("{0} {1}", dealer.FirstName, dealer.LastName));
				var to = new MailAddress(dealer.email);
				InsertQueuedEmail(from, to, cc, bcc, localizedMessageTemplate.Subject, emailBody, 0);
			}
		}

		public static string ReplaceMessageTemplateTokensForDealerNotification(string template, string make, string model, string maximumMileage, string yearFrom, string yearTo, string minPrice,
			string maxPrice, string body, string transmission, string engine, string driverType, string firstName, string lastName, string phoneNo, string email)
		{
			var tokens = new NameValueCollection();
			tokens.Add("Store.Url", SettingManager.StoreUrl);
			tokens.Add("Vehicle.Make", make);
			tokens.Add("Vehicle.Model", model);
			tokens.Add("Vehicle.MaximumMileage", maximumMileage);
			tokens.Add("Vehicle.YearFrom", yearFrom);
			tokens.Add("Vehicle.YearTo", yearTo);
			tokens.Add("Vehicle.MinPrice", minPrice);
			tokens.Add("Vehicle.MaxPrice", maxPrice);
			tokens.Add("Vehicle.Body", body);
			tokens.Add("Vehicle.Transmission", transmission);
			tokens.Add("Vehicle.Engine", engine);
			tokens.Add("Vehicle.DriveType", driverType);
			tokens.Add("Customer.FirstName", firstName);
			tokens.Add("Customer.LastName", lastName);
			tokens.Add("Customer.PhoneNo", phoneNo);
			tokens.Add("Customer.Email", email);
			//tokens.Add("Dealer.Name", AdminEmailAddress);

			foreach(string token in tokens.Keys)
			{
				template = template.Replace(String.Format("%{0}%", token), tokens[token]);
			}

			return template;
		}

		public static string ReplaceMessageTemplateTokensForAdmin(Customer customer, string code, string here, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("Customer.FirstName", customer.FirstName);
            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.LastName", HttpUtility.HtmlEncode(customer.LastName));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.Name));
            tokens.Add("Customer.Password", HttpUtility.HtmlEncode(customer.Password));
            tokens.Add("Customer.Phone", HttpUtility.HtmlEncode(customer.Phone));
            tokens.Add("Customer.ZipCode", HttpUtility.HtmlEncode(customer.zip));
            tokens.Add("Customer.Code", HttpUtility.HtmlEncode(code));
            tokens.Add("Customer.YourHere", HttpUtility.HtmlEncode(here));

            //string passwordRecoveryUrl = string.Empty;
            //passwordRecoveryUrl = string.Format("{0}passwordrecovery.aspx?prt={1}&email={2}", SettingManager.StoreUrl, customer.PasswordRecoveryToken, customer.Email);
            //tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryUrl);

            //string accountActivationUrl = string.Empty;
            //accountActivationUrl = string.Format("{0}accountactivation.aspx?act={1}&email={2}", SettingManager.StoreUrl, customer.AccountActivationToken, customer.Email);
            //tokens.Add("Customer.AccountActivationURL", accountActivationUrl);

            foreach (string token in tokens.Keys)
            {
                if (template.Contains("%" + token + "%"))
                    template = template.Replace("%" + token + "%", tokens[token]);
            }

            return template;
        }
        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static void SendCustomerPasswordRecoveryMessage(Edrive.Models.Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string templateName = "Customer.PasswordRecovery";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
         
            //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BCCEmailAddresses;
            string cc = AdminEmailAddress;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.Name);
          InsertQueuedEmail( from, to, cc, bcc, subject, body,0);
           
        }
       

        public static void SendCustomerPasswordRecoveryMessage(Edrive.Edrivie_Service_Ref.Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string templateName = "Customer.PasswordRecovery";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);

            //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BCCEmailAddresses;
            string cc = AdminEmailAddress;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.email, customer.Name);
            InsertQueuedEmail(from, to, cc, bcc, subject, body, 0);

        }
        public static string FormatContactUsFormText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            //text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
            
            return text;
        }


        public static List<MessageTemplate>  GetAllMessageTemplates()
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
               return _entity.MessageTemplate.Where(m=>m.Deleted==false|| m.Deleted==null).ToList();
            }

 
        }
        //public static List<MessageTemplate> GetAllMessageTemplates()
        //{
        //    using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
        //    {
        //        return _entity.MessageTemplate.ToList();
        //    }


        //}



         public             static MessageTemplate GetMessageTemplate_by_tempId(int msgTempId)
        {
              using(eDriveAutoWebEntities _entity=new eDriveAutoWebEntities ())
              {
                 return _entity.MessageTemplate.FirstOrDefault(m => m.MessageTemplateID == msgTempId);
              }

        }
        /// <summary>
        /// to update the Details for Message Template
        /// </summary>
        /// <param name="model"></param>
         public static void Update_LocalizedMessageTemplate_by_templateID(MessageTemplateLocalized model)
         {
             using(eDriveAutoWebEntities _entity=new eDriveAutoWebEntities ())
             {
                var prevModel_MessageTemplateLocalized= _entity.MessageTemplateLocalized.First(m=>m.MessageTemplateLocalizedID==model.MessageTemplateLocalizedID);
                prevModel_MessageTemplateLocalized.BCCEmailAddresses = model.BCCEmailAddresses;
                prevModel_MessageTemplateLocalized.Body = model.Body;
                prevModel_MessageTemplateLocalized.IsActive = model.IsActive;
                prevModel_MessageTemplateLocalized.Subject = model.Subject;
                _entity.SaveChanges();
             }

         }

         public static void Deleted_MessageTemplate(int MessageTemplateID)
         {
               using(eDriveAutoWebEntities _entity=new eDriveAutoWebEntities ())
             {
                var template= _entity.MessageTemplate.First(m => m.MessageTemplateID == MessageTemplateID);
                template.Deleted = true;
                _entity.SaveChanges();
               }
            
         }

         public static MessageTemplate AddMessgaeTemplate(_MessageTemplateLocalized model)
         {
             using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
             {
                 MessageTemplate tempLate = new MessageTemplate();
                 tempLate.Name=  model.TemplateName ;
                 _entity.MessageTemplate.AddObject(tempLate);
                     var msgTempLocalised= new MessageTemplateLocalized ();
                     msgTempLocalised.MessageTemplateID = tempLate.MessageTemplateID;
                msgTempLocalised.BCCEmailAddresses = model.BCCEmailAddresses;
                msgTempLocalised.Body = model.Body;
                msgTempLocalised.IsActive = model.IsActive;
                msgTempLocalised.Subject = model.Subject;
                 _entity.MessageTemplateLocalized.AddObject(msgTempLocalised);
                 _entity.SaveChanges();
                 return tempLate;
             }
         }

         
    }
}