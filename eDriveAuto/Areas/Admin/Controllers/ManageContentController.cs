using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Core.Model;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using System.Web;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageContentController : Controller
    {
	    private readonly IEmailTemplateService _emailTemplateService;

		public ManageContentController(IEmailTemplateService emailTemplateService)
		{
			_emailTemplateService = emailTemplateService;
		}

        //
        // GET: /Admin/ManageContent/

        public ActionResult Index()
        {
            return View("Newsletters");
        }

		public ActionResult FeaturedSeller()
		{
			string rootFolder = HttpContext.Request.MapPath("~/");
			string fullPath = Path.Combine(rootFolder, "Content\\Images\\Dealer\\FeaturedSellerIcon.gif");
			string imagePath = "~/Content/Images/Dealer/FeaturedSellerIcon.gif";

			if(!System.IO.File.Exists(fullPath))
				imagePath = null;

			@ViewBag.ImagePath = imagePath;

			return View();
		}

		[HttpPost]
		public ActionResult FeaturedSeller(FormCollection collection, HttpPostedFileBase filePath)
		{
			string rootFolder = HttpContext.Request.MapPath("~/");
			string imagePath = Path.Combine(rootFolder, "Content\\Images\\Dealer\\FeaturedSellerIcon.gif");
			string virtualPath = string.Format("~/Content/Images/Dealer/FeaturedSellerIcon.gif?r={0}", new Random().Next(1, 50));
			
			if(filePath != null && filePath.ContentLength > 0)
			{

				Image resizedImage = null;

				try
				{
					if(System.IO.File.Exists(imagePath))
						System.IO.File.Delete(imagePath);

					resizedImage = Common_Methods.ResizeByWidth(Image.FromStream(filePath.InputStream), 188);
					resizedImage.Save(imagePath, ImageFormat.Gif);
					ViewBag.Message = "<i class='success-message'>Image successfully saved.</i>";
					ViewBag.ImagePath = virtualPath;
				}
				catch
				{
					ViewBag.Message = "<i class='error-message'>Error occurred while saving the image.</i>";
				}
				finally
				{
					if(resizedImage != null) resizedImage.Dispose();
				}
			}
         	else
			{
				if(System.IO.File.Exists(imagePath))
					ViewBag.ImagePath = virtualPath;

				ViewBag.Message = "<i class='error-message'>Please choose a file.</i>";
			}

			return View();
		}

        #region Press



        public ActionResult Press()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var _newes = _enitity.Nop_News.OrderByDescending(m => m.CreatedOn).ToList();
                return View(_newes);
            }
        }
        /// <summary>
        /// To add new press record
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult AddPress()
        {
            return View();
        }
        /// <summary>
        /// To save new press record
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPress(Models.Nop_News Model)
        {
            Model.Short = Model.Short ?? "";
            Model.Full = Model.Full ?? "";

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var newPressNews = new Models.Nop_News { AllowComments = true, CreatedOn = DateTime.Now, Full = Model.Full ?? "", LanguageID = 0, Published = Model.Published, Short = Model.Short ?? "", Title = Model.Title };
                _enitity.Nop_News.AddObject(newPressNews);
                _enitity.SaveChanges();

                TempData["Msg"] = "Record added successfullly.";
                return RedirectToAction("EditPress", new { id = newPressNews.NewsID });

            }
        }

        /// <summary>
        /// to edit  content of Press Module
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPress(Int32 id)
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var prevPRess = _enitity.Nop_News.First(m => m.NewsID == id);
                return View(prevPRess);
            }
        }
        /// <summary>
        /// Updates the content of Press
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPress(Models.Nop_News Model)
        {
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var prevPRess = _enitity.Nop_News.First(m => m.NewsID == Model.NewsID);
                prevPRess.AllowComments = true;
                prevPRess.Full = Model.Full ?? "";
                prevPRess.Published = Model.Published;
                prevPRess.Short = Model.Short ?? "";
                prevPRess.Title = Model.Title;

                _enitity.SaveChanges();

                TempData["Msg"] = "Record Updadted successfullly.";
                return EditPress(prevPRess.NewsID);

            }

        }

        /// <summary>
        /// To delete the press records
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePress(Models.Nop_News Model)
        {
            try
            {
                using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
                {
                    var prevPRess = _enitity.Nop_News.First(m => m.NewsID == Model.NewsID);
                    _enitity.Nop_News.DeleteObject(prevPRess);
                    _enitity.SaveChanges();

                    TempData["Msg"] = "Record deleted successfully";
                    return RedirectToAction("Press");
                }

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error: " + ex.Message;
                return RedirectToAction("EditPress", new { id = Model.NewsID });
            }

        }

        #endregion
        #region Emanagement

        public ActionResult EManagement()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var model = _enitity.ED_EManagement.Where(m => m.Deleted == false).ToList();

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult EManagement(ED_EManagement model)
        {
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                return View();
            }
        }

        public ActionResult editEManagement(Int32 id)
        {
            var managementid = id;
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var model = _enitity.ED_EManagement.First(m => m.Deleted == false && m.ManagementID == managementid);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult DelteImage_Image_for_EManagement(ED_EManagement model)
        {
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var mgmt = _enitity.ED_EManagement.First(M => M.ManagementID == model.ManagementID);
                mgmt.ImageID = 0;
                TempData["Msg"] = "Image removed successfully.";
                _enitity.SaveChanges();

            }
            return RedirectToAction("editEManagement", new { id = model.ManagementID });
        }

        [HttpPost]
        public ActionResult editEManagement(ED_EManagement model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var partImageID = model.ImageID ?? 0;
                #region "SaveFile"
                if (Request.Files["Pic"] != null)
                {
                    if (Request.Files["Pic"].ContentLength > 0)
                    {
                        var Length = Request.Files["Pic"].ContentLength;
                        byte[] filsbyte = new byte[Length];
                        Request.Files["Pic"].InputStream.Read(filsbyte, 0, Length);
                        var ext = Path.GetExtension(Request.Files["Pic"].FileName);
                        if (ext.Contains("."))
                            ext = ext.Substring(ext.LastIndexOf('.') + 1);
                        Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                        _enitity.Nop_Picture.AddObject(newPartenerImage);
                        _enitity.SaveChanges();
                        partImageID = newPartenerImage.PictureID;
                    }


                }
                #endregion


                model.UpdatedOn = DateTime.Now;


                var UpdateMOdel = _enitity.ED_EManagement.FirstOrDefault(m => m.ManagementID == model.ManagementID);

                UpdateMOdel.ImageID = partImageID;
                UpdateMOdel.DisplayOrder = model.DisplayOrder;
                UpdateMOdel.Published = model.Published;
                UpdateMOdel.ShortDesc = model.ShortDesc;
                UpdateMOdel.Title = model.Title;
                UpdateMOdel.UpdatedOn = DateTime.Now;
                _enitity.SaveChanges();

                TempData["Msg"] = "Record Updated successfully.";
                return editEManagement(model.ManagementID);

            }
            // return View();
        }

        [HttpPost]
        public ActionResult Delete_EManagement(ED_EManagement model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var mgmt = _enitity.ED_EManagement.FirstOrDefault(m => m.ManagementID == model.ManagementID);
                mgmt.Deleted = true;
                _enitity.SaveChanges();
                TempData["Msg"] = "Record Update successfully.";
                return RedirectToAction("EManagement");

            }
            // return View();
        }


        public ActionResult AddEManagement()
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                return View(new ED_EManagement());
            }
        }

        [HttpPost]
        public ActionResult AddEManagement(ED_EManagement model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var partImageID = 0;
                #region "SaveFile"
                if (Request.Files["Pic"] != null)

                    if (Request.Files["Pic"].ContentLength > 0)
                    {
                        var Length = Request.Files["Pic"].ContentLength;
                        byte[] filsbyte = new byte[Length];
                        Request.Files["Pic"].InputStream.Read(filsbyte, 0, Length);
                        var ext = Path.GetExtension(Request.Files["Pic"].FileName);
                        if (ext.Contains("."))
                            ext = ext.Substring(ext.LastIndexOf('.') + 1);
                        Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                        _enitity.Nop_Picture.AddObject(newPartenerImage);
                        _enitity.SaveChanges();
                        partImageID = newPartenerImage.PictureID;
                    }


                #endregion
                model.ImageID = partImageID;
                model.CreatedOn = DateTime.Now;
                model.UpdatedOn = DateTime.Now;

                model.Deleted = false;
                _enitity.ED_EManagement.AddObject(model);
                _enitity.SaveChanges();



                TempData["Msg"] = "Record added successfully.";
                return RedirectToAction("editEManagement", new { id = model.ManagementID });

            }
            // return View();
        }
        #endregion

        #region Topics

        public ActionResult AddTopic()
        {


            return View(new _Nop_TopicLocalized());

        }
        [HttpPost]
        public ActionResult AddTopic(_Nop_TopicLocalized Model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                if (_enitity.Nop_Topic.Any(m => m.Name == Model.TopicName))
                {
                    ViewData["Msg"] = "Error: This Topic Name already exist";
                    return View();
                }
                var newTopic = new Nop_Topic { Name = Model.TopicName };
                _enitity.Nop_Topic.AddObject(newTopic);
                var _TopicDetail = new Models.Nop_TopicLocalized();
                _TopicDetail.TopicID = newTopic.TopicID;
                _TopicDetail.Body = Model.Body ?? "";
                _TopicDetail.Title = Model.Title ?? "";
                _TopicDetail.MetaDescription = Model.MetaDescription ?? "";
                _TopicDetail.MetaKeywords = Model.MetaKeywords ?? "";
                _TopicDetail.MetaTitle = Model.MetaTitle ?? "";
                _TopicDetail.UpdatedOn = DateTime.UtcNow;
                _TopicDetail.CreatedOn = DateTime.UtcNow;
                _enitity.Nop_TopicLocalized.AddObject(_TopicDetail);

                _enitity.SaveChanges();
                TempData["Msg"] = "Record added successfully.";
                return RedirectToAction("EditTopic", new { id = _TopicDetail.TopicID });
            }

        }

        public ActionResult Topic()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var lstTopics = _enitity.Nop_Topic.ToList();

                return View(lstTopics);
            }
        }
        public ActionResult EditTopic(Int32 id)
        {
            var topicid = id;
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var DetailsTopics = _enitity.Nop_TopicLocalized.First(m => m.TopicID == topicid);
                var _TopicDetail = new _Nop_TopicLocalized();
                _TopicDetail.Body = DetailsTopics.Body;

                _TopicDetail.MetaDescription = DetailsTopics.MetaDescription;
                _TopicDetail.MetaKeywords = DetailsTopics.MetaKeywords;
                _TopicDetail.MetaTitle = DetailsTopics.MetaTitle;
                _TopicDetail.Title = DetailsTopics.Title;
                _TopicDetail.UpdatedOn = DetailsTopics.UpdatedOn;
                _TopicDetail.CreatedOn = DetailsTopics.CreatedOn;
                _TopicDetail.TopicName = DetailsTopics.Nop_Topic.Name;

                _TopicDetail.TopicID = DetailsTopics.TopicID;
                _TopicDetail.TopicLocalizedID = DetailsTopics.TopicLocalizedID;
                return View(_TopicDetail);
            }

        }
        [HttpPost]
        public ActionResult EditTopic(_Nop_TopicLocalized Model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var _TopicDetail = _enitity.Nop_TopicLocalized.First(m => m.TopicLocalizedID == Model.TopicLocalizedID);
                _TopicDetail.Body = Model.Body ?? "";
                _TopicDetail.Title = Model.Title ?? "";
                _TopicDetail.MetaDescription = Model.MetaDescription ?? "";
                _TopicDetail.MetaKeywords = Model.MetaKeywords ?? "";
                _TopicDetail.MetaTitle = Model.MetaTitle ?? "";
                _TopicDetail.UpdatedOn = DateTime.UtcNow;
                _enitity.SaveChanges();

                return EditTopic(Model.TopicID);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(_Nop_TopicLocalized Model)
        {
            var id = Model.TopicLocalizedID;
            try
            {

                var TopicLocalizedID = id;
                using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
                {
                    var topicdetail = _enitity.Nop_TopicLocalized.First(m => m.TopicLocalizedID == TopicLocalizedID);
                    _enitity.Nop_TopicLocalized.DeleteObject(topicdetail);
                    var topic = _enitity.Nop_Topic.First(m => m.TopicID == topicdetail.TopicID);
                    _enitity.Nop_Topic.DeleteObject(topic);
                    _enitity.SaveChanges();
                    TempData["Msg"] = "Record deleted successfully.";

                    return RedirectToAction("Topic");
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error on deleting.=" + ex.Message;
                return EditTopic(id);
            }

        }
        #endregion

        #region Newsletters

        /// <summary>
        /// To view the newsletter and manage the newsletter
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletters()
        {

            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var lstCampaign = _enitity.Nop_Campaign.OrderByDescending(m => m.CampaignID).ToList();

                return View(lstCampaign);
            }
        }

        /// <summary>
        /// When Add new and Export button are clicked.
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="AddNew"></param>
        /// <param name="Export"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Newsletters(Nop_Campaign Model, String AddNew, String Export)
        {
            if (AddNew != null)//---Redirect to add new newsletter
            {
                return RedirectToAction("AddNewNewsLetter");
            }
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                // When export to csv is clicked
                if (Export != null)
                {
                   String[] emails= _enitity.Customer.Where(m => m.Deleted == false && m.Customer_Type.RoleName == "Customer" && m.IsNewsLetter == true).Select(m => m.Email).ToArray();
                  String emailStr="";
                    foreach (string item in emails)
                    {
                        emailStr=","+item;
                    }
                   if(emailStr.IndexOf(',')>=0)
                   {
                       emailStr=emailStr.Substring(1);
                   }
                    byte[] bytearray = Encoding.ASCII.GetBytes(emailStr);
                   MemoryStream csvfile = new MemoryStream();
                   csvfile.Write(bytearray,0,bytearray.Length);
                   return File(bytearray, "text", "newsletter_emails_" + DateTime.Now.ToString("yyyy-dd-MM-hh-mm-ss") + ".txt");

                }




                var lstCampaign = _enitity.Nop_Campaign.OrderByDescending(m => m.CampaignID).ToList();
                return View(lstCampaign);
            }
        }


        /// <summary>
        /// Sends a campaign to specified email
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <param name="email">Email</param>
        public static void SendCampaign(int campaignId, string email)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var campaign = _enitity.Nop_Campaign.FirstOrDefault(m => m.CampaignID == campaignId);
                if (campaign == null)
                {
                    throw new Exception("Campaign could not be loaded");
                }

                string subject = campaign.Subject;
                string body = campaign.Body;
                var from = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                var to = new MailAddress(email);
                MessageManager.SendEmail(subject, body, from, to);
            }
        }
        public ActionResult AddNewNewsLetter()
        {
            return View();
        }
        /// <summary>
        /// It saves the new newsletter
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewNewsLetter(Nop_Campaign model)
        {
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var Campaign = new Nop_Campaign();
                Campaign.Body = model.Body;
                Campaign.Name = model.Name;
                Campaign.Subject = model.Subject;
                Campaign.CreatedOn = DateTime.Now;
                _enitity.Nop_Campaign.AddObject(Campaign);
                _enitity.SaveChanges();
                TempData["Msg"] = "Record Added successfully.";
                return RedirectToAction("EditNewsLetter", new { id = (Campaign.CampaignID) });
            }
        }
        /// <summary>
        /// To edit the newsletter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditNewsLetter(Int32 id)
        {

            ViewData["allowedTokens"] = MessageManager.GetListOfCampaignAllowedTokens();

            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var Campaign = _enitity.Nop_Campaign.First(m => m.CampaignID == id);
                return View(Campaign);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditNewsLetter(Nop_Campaign model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var Campaign = _enitity.Nop_Campaign.First(m => m.CampaignID == model.CampaignID);
                Campaign.Body = model.Body;
                Campaign.Name = model.Name;
                Campaign.Subject = model.Subject;
                _enitity.SaveChanges();
                ViewData["Msg"] = "Record updated successfully.";
                ViewData["allowedTokens"] = MessageManager.GetListOfCampaignAllowedTokens();
                return EditNewsLetter(model.CampaignID);
            }
        }

        [HttpPost]
        public ActionResult DeleteNewsLetter(Nop_Campaign model)
        {

            using (eDriveAutoWebEntities _enitity = new Models.eDriveAutoWebEntities())
            {
                var Campaign = _enitity.Nop_Campaign.First(m => m.CampaignID == model.CampaignID);

                _enitity.Nop_Campaign.DeleteObject(Campaign);
                _enitity.SaveChanges();
                TempData["Msg"] = "Record Deleted successfully.";
                return RedirectToAction("Newsletters");
            }
        }

        [HttpPost]
        public ActionResult SendNewsLetterMail(String SendTestEmail, String SendMassEmail, _SendNewsLetterMail Model)
        {
            try
            {
                if (SendTestEmail != null)// send the Test mail code
                {
                    SendCampaign(Model.CampaignID, Model.Email);
                    //-------send test email 
                    TempData["Msg"] = "Email has been successfully sent.";
                    return RedirectToAction("EditNewsLetter", new { id = Model.CampaignID });
                }
                if (SendMassEmail != null)
                {
                    using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                    {
                        //-------only customer not the dealer will get the newsletter------------
                        var subscriptions = _entity.Customer.Where(m => m.Deleted == false &&
                           m.Customer_Type.RoleName == "Customer" && m.IsNewsLetter == true).ToList();

                        int totalEmailsSent = SendCampaign(Model.CampaignID, subscriptions);
                        TempData["Msg"] = "Emails has been successfully sent to " + totalEmailsSent + " customers.";
                        return RedirectToAction("EditNewsLetter", new { id = Model.CampaignID });
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error: " + ex.Message;
            }
            return RedirectToAction("EditNewsLetter", new { id = Model.CampaignID });

        }

        public int SendCampaign(int campaignId, List<Models.Customer> subscriptions)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                int totalEmailsSent = 0;

                var campaign = _entity.Nop_Campaign.FirstOrDefault(m => m.CampaignID == campaignId);

                if (campaign == null)
                {
                    throw new Exception("Campaign could not be loaded");
                }

                foreach (Models.Customer subscription in subscriptions)
                {

                    string subject = MessageManager.ReplaceMessageTemplateTokens_for_newsletter(subscription, campaign.Subject);
                    string body = MessageManager.ReplaceMessageTemplateTokens_for_newsletter(subscription, campaign.Body);
                    var from = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                    var to = new MailAddress(subscription.Email);
                    try
                    {
                        MessageManager.InsertQueuedEmail(from, to, string.Empty, string.Empty, subject, body, 3);
                        totalEmailsSent++;
                    }
                    catch (Exception ex)
                    {


                    }

                }
                return totalEmailsSent;
            }

        }

        /// <summary>
        /// To dellete the the news letter
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteNewsLetter(_SendNewsLetterMail Model)
        {
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    Nop_Campaign campaign = _entity.Nop_Campaign.First(m => m.CampaignID == Model.CampaignID);
                    _entity.Nop_Campaign.DeleteObject(campaign);
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record Deleted successfully.";
                    return RedirectToAction("Newsletters");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditNewsLetter", new { id = Model.CampaignID });
            }

        }
        #endregion
        #region ManageFAQ
        /// <summary>
        /// It show the Users Faqs
        /// </summary>
        /// <returns></returns>
        public ActionResult UserFAQs()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var lstUserFaqs = _entity.ED_Faq.Where(m => m.IsActive).OrderBy(m => m.OrderId).ToList();

                return View(lstUserFaqs);
            }
        }
        /// <summary>
        /// To edit the user faq
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUserFAQs(long id)
        {
            ViewData["Msg"] = TempData["Msg"];
            var FAQid = id;
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var UserFaqs = _entity.ED_Faq.First(m => m.FaqId == FAQid);
                return View(UserFaqs);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserFAQs(ED_Faq model)
        {

            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var UserFaqs = _entity.ED_Faq.First(m => m.FaqId == model.FaqId);
                UserFaqs.Answer = model.Answer ?? "";
                UserFaqs.Question = model.Question;
                UserFaqs.UpdatedOn = DateTime.Now;
                _entity.SaveChanges();
                TempData["Msg"] = "Record updated successfully.";

                return EditUserFAQs(model.FaqId);
            }
        }
        /// <summary>
        /// return 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUserFAQs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserFAQs(ED_Faq model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    model.CreatedOn = DateTime.Now;
                    model.UpdatedOn = DateTime.Now;
                    model.Answer = model.Answer ?? "";
                    model.IsActive = true;
                    model.OrderId = _entity.ED_Faq.Max(m => m.OrderId) + 1;
                    _entity.ED_Faq.AddObject(model);
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record added successfully.";
                    return RedirectToAction("EditUserFAQs", new { id = model.FaqId });
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }

        public ActionResult DeleteUserFAQs(ED_Faq model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var UserFaqs = _entity.ED_Faq.First(m => m.FaqId == model.FaqId);
                    UserFaqs.IsActive = false;
                    _entity.SaveChanges();

                    TempData["Msg"] = "Record Deleted successfully.";
                    return RedirectToAction("UserFAQs");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditUserFAQs", new { id = model.FaqId });
            }
        }



        public ActionResult DealerFAQs()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var lstUserFaqs = _entity.ED_DealersFaq.Where(m => m.IsActive).OrderBy(m => m.OrderId).ToList();

                return View(lstUserFaqs);
            }
        }

        public ActionResult EditDealerFAQs(long id)
        {
            ViewData["Msg"] = TempData["Msg"];
            var FAQid = id;
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var DealerFaqs = _entity.ED_DealersFaq.First(m => m.FaqId == FAQid);
                return View(DealerFaqs);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDealerFAQs(ED_DealersFaq model)
        {

            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var DealerFaqs = _entity.ED_DealersFaq.First(m => m.FaqId == model.FaqId);
                DealerFaqs.Answer = model.Answer ?? "";
                DealerFaqs.Question = model.Question;
                DealerFaqs.UpdatedOn = DateTime.Now;
                _entity.SaveChanges();
                TempData["Msg"] = "Record updated successfully.";

                return EditDealerFAQs(model.FaqId);
            }
        }
        /// <summary>
        /// return 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDealerFAQs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDealerFAQs(ED_DealersFaq model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    model.CreatedOn = DateTime.Now;
                    model.UpdatedOn = DateTime.Now;
                    model.Answer = model.Answer ?? "";
                    model.IsActive = true;
                    model.OrderId = _entity.ED_DealersFaq.Max(m => m.OrderId) + 1;
                    _entity.ED_DealersFaq.AddObject(model);
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record added successfully.";
                    return RedirectToAction("EditDealerFAQs", new { id = model.FaqId });
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }

        public ActionResult DeleteDealerFAQs(ED_DealersFaq model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var DealerFaqs = _entity.ED_DealersFaq.First(m => m.FaqId == model.FaqId);
                    DealerFaqs.IsActive = false;
                    _entity.SaveChanges();

                    TempData["Msg"] = "Record Deleted successfully.";
                    return RedirectToAction("DealerFAQs");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditDealerFAQs", new { id = model.FaqId });
            }
        }
        public ActionResult MoveUpRecord(Int32 id)
        {
            var faqid = id;
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var faq_to_moveUP = _entity.ED_DealersFaq.First(m => m.FaqId == faqid);
                    var prevProduct = _entity.ED_DealersFaq.Where(m => m.IsActive == true && m.OrderId < faq_to_moveUP.OrderId).OrderByDescending(m => m.OrderId).First();
                    //swap order id with product prior to it
                    var tem = prevProduct.OrderId;
                    prevProduct.OrderId = faq_to_moveUP.OrderId;
                    faq_to_moveUP.OrderId = tem;
                    _entity.SaveChanges();
                    var newUPdatedList = _entity.ED_DealersFaq.Where(m => m.IsActive == true).OrderBy(m => m.OrderId).ToList();
                    return View("_ShowDealers", newUPdatedList);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }


        }
        public ActionResult MoveDownRecord(Int32 id)
        {
            var faqid = id;
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var faq_to_moveUP = _entity.ED_DealersFaq.First(m => m.FaqId == faqid);
                    var nextProduct = _entity.ED_DealersFaq.Where(m => m.IsActive == true && m.OrderId > faq_to_moveUP.OrderId).OrderBy(m => m.OrderId).First();
                    //swap order id with product prior to it
                    var tem = nextProduct.OrderId;
                    nextProduct.OrderId = faq_to_moveUP.OrderId;
                    faq_to_moveUP.OrderId = tem;
                    _entity.SaveChanges();
                    var newUPdatedList = _entity.ED_DealersFaq.Where(m => m.IsActive == true).OrderBy(m => m.OrderId).ToList();
                    return View("_ShowDealers", newUPdatedList);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }


        }


        public ActionResult MoveUpUserFAQRecord(Int32 id)
        {
            var faqid = id;
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var faq_to_moveUP = _entity.ED_Faq.First(m => m.FaqId == faqid);
                    var prevProduct = _entity.ED_Faq.Where(m => m.IsActive == true && m.OrderId < faq_to_moveUP.OrderId).OrderByDescending(m => m.OrderId).First();
                    //swap order id with product prior to it
                    var tem = prevProduct.OrderId;
                    prevProduct.OrderId = faq_to_moveUP.OrderId;
                    faq_to_moveUP.OrderId = tem;
                    _entity.SaveChanges();
                    var newUPdatedList = _entity.ED_Faq.Where(m => m.IsActive == true).OrderBy(m => m.OrderId).ToList();
                    return View("_ShowUserFAQs", newUPdatedList);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }


        }
        public ActionResult MoveDownUserFAQRecord(Int32 id)
        {
            var faqid = id;
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var faq_to_moveUP = _entity.ED_Faq.First(m => m.FaqId == faqid);
                    var nextProduct = _entity.ED_Faq.Where(m => m.IsActive == true && m.OrderId > faq_to_moveUP.OrderId).OrderBy(m => m.OrderId).First();
                    //swap order id with product prior to it
                    var tem = nextProduct.OrderId;
                    nextProduct.OrderId = faq_to_moveUP.OrderId;
                    faq_to_moveUP.OrderId = tem;
                    _entity.SaveChanges();
                    var newUPdatedList = _entity.ED_Faq.Where(m => m.IsActive == true).OrderBy(m => m.OrderId).ToList();
                    return View("_ShowUserFAQs", newUPdatedList);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }


        }


        #endregion

        #region Testimonial
        public ActionResult Testimonials()
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                ViewData["Msg"] = TempData["Msg"];
                return View(_entity.ED_Testimonials.Where(m => m.IsActive).OrderByDescending(m => m.CreatedOn).ToList());

            }
        }
        public ActionResult AddTestimonials()
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                return View();
            }
        }
        [HttpPost]
        public ActionResult AddTestimonials(ED_Testimonials model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var partImageID = 0;
                    #region "SaveFile"
                    if (Request.Files["Pic"] != null)

                        if (Request.Files["Pic"].ContentLength > 0)
                        {
                            var Length = Request.Files["Pic"].ContentLength;
                            byte[] filsbyte = new byte[Length];
                            Request.Files["Pic"].InputStream.Read(filsbyte, 0, Length);
                            var ext = Path.GetExtension(Request.Files["Pic"].FileName);
                            if (ext.Contains("."))
                                ext = ext.Substring(ext.LastIndexOf('.') + 1);
                            Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                            _entity.Nop_Picture.AddObject(newPartenerImage);
                            _entity.SaveChanges();
                            partImageID = newPartenerImage.PictureID;
                        }
                    #endregion
                    model.PictureId = partImageID;
                    _entity.ED_Testimonials.AddObject(model);
                    model.IsActive = true;
                    model.UpdatedOn = model.CreatedOn = DateTime.Now;
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record added successfully.";

                    return RedirectToAction("Testimonials");
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }

        public ActionResult EditTestimonials(Int32 id)
        {
            try
            {
                ViewData["Msg"] = TempData["Msg"];

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    return View(_entity.ED_Testimonials.First(m => m.TId == id));
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult EditTestimonials(ED_Testimonials model)
        {
            try
            {
                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var partImageID = model.PictureId;
                    #region "SaveFile"
                    if (Request.Files["Pic"] != null)

                        if (Request.Files["Pic"].ContentLength > 0)
                        {
                            var Length = Request.Files["Pic"].ContentLength;
                            byte[] filsbyte = new byte[Length];
                            Request.Files["Pic"].InputStream.Read(filsbyte, 0, Length);
                            var ext = Path.GetExtension(Request.Files["Pic"].FileName);
                            if (ext.Contains("."))
                                ext = ext.Substring(ext.LastIndexOf('.') + 1);
                            Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                            _entity.Nop_Picture.AddObject(newPartenerImage);
                            _entity.SaveChanges();
                            partImageID = newPartenerImage.PictureID;
                        }
                    #endregion
                    var upd_Model = _entity.ED_Testimonials.FirstOrDefault(m => m.TId == model.TId);
                    upd_Model.UpdatedOn = DateTime.Now;
                    upd_Model.PictureId = partImageID;
                    upd_Model.Name = model.Name;
                    upd_Model.TContent = model.TContent;
                    upd_Model.Address = model.Address;
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record updated successfully.";

                    return RedirectToAction("Testimonials");

                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }
        public ActionResult DeleteImage_Image_for_Testimonial(ED_Testimonials model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var delobj = _entity.Nop_Picture.FirstOrDefault(m => m.PictureID == model.PictureId);
                    if (delobj != null)
                        _entity.Nop_Picture.DeleteObject(delobj);
                    var upd_Model = _entity.ED_Testimonials.FirstOrDefault(m => m.TId == model.TId);
                    upd_Model.PictureId = 0;//resest to default image;
                    _entity.SaveChanges();

                }
                TempData["Msg"] = "Image Deleted sucessfully";
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;

            }
            return RedirectToAction("EditTestimonials", new { id = model.TId });
        }

        public ActionResult Delete_Testimonials(ED_Testimonials model)
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {

                    var upd_Model = _entity.ED_Testimonials.FirstOrDefault(m => m.TId == model.TId);
                    upd_Model.IsActive = false;//reset to default image;
                    _entity.SaveChanges();

                }
                TempData["Msg"] = "Record Deleted sucessfully";
                return RedirectToAction("Testimonials");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditTestimonials", new { id = model.TId });
            }

        }

        #endregion


        #region EGears

        /// <summary>
        /// To manage the EGEars list
        /// </summary>
        /// <returns></returns>
        public ActionResult EGears()
        {
            ViewData["Msg"] = TempData["Msg"];
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var lst = _entity.ED_EGear.Where(m => m.Deleted == false).OrderBy(m => m.DisplayOrder).ToList();
                    return View(lst);
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }

        }
        public ActionResult AddEGear()
        {
            return View();
        }


        /// <summary>
        /// Save the Egear record
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult AddEGear(ED_EGear Model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View();
                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {

                    Model.CreatedOn = Model.UpdatedOn = DateTime.Now;
                    Model.Deleted = false;
                    Model.ImageID = 0;

                    //-----------save picture----------------

                    if (Request.Files["Pic"] != null)
                    {
                        if (string.IsNullOrEmpty(Request.Files["Pic"].FileName) == false && Request.Files["Pic"].ContentLength > 0)
                        {
                            var newPicture = PictureManager.SavePicture(Request.Files["Pic"]);
                            if (newPicture != null)
                                Model.ImageID = newPicture.PictureID;
                        }
                    }


                    _entity.ED_EGear.AddObject(Model);
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record Added successfully.";
                }

                return RedirectToAction("EGears");
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }

        }


        public ActionResult EditEGears(Int32 id)
        {
            try
            {
                ViewData["Msg"] = TempData["Msg"];

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var egear = _entity.ED_EGear.First(m => m.eGearID == id);
                    return View(egear);
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();
            }
        }
        /// <summary>
        /// To Save the updated changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEGears(ED_EGear Model)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                Model.UpdatedOn = DateTime.Now;


                //-----------save picture----------------

                if (Request.Files["Pic"] != null)
                {
                    if (string.IsNullOrEmpty(Request.Files["Pic"].FileName) == false && Request.Files["Pic"].ContentLength > 0)
                    {
                        var newPicture = PictureManager.SavePicture(Request.Files["Pic"]);
                        if (newPicture != null)
                            Model.ImageID = newPicture.PictureID;
                    }
                }

                var egearToUpdate = _entity.ED_EGear.First(m => m.eGearID == Model.eGearID);
                egearToUpdate.UpdatedOn = Model.UpdatedOn;
                egearToUpdate.DisplayOrder = Model.DisplayOrder;
                egearToUpdate.ImageID = Model.ImageID;
                egearToUpdate.Price = Model.Price;
                egearToUpdate.ProductName = Model.ProductName;
                egearToUpdate.Published = Model.Published;
                egearToUpdate.Qty = Model.Qty;
                egearToUpdate.ShortDesc = Model.ShortDesc;
                _entity.SaveChanges();
                TempData["Msg"] = "Record updated successfully.";
                return RedirectToAction("EGears");
            }

        }
        /// <summary>
        /// This method deleltes the image for  EGear when we edit the image.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEGearImage(ED_EGear model)
        {
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var egear = _entity.ED_EGear.First(m => m.eGearID == model.eGearID);
                    var img = _entity.Nop_Picture.First(m => m.PictureID == model.ImageID);
                    _entity.Nop_Picture.DeleteObject(img);
                    egear.ImageID = 0;
                    _entity.SaveChanges();

                    TempData["Msg"] = "Image Deleted successfully.";

                    return RedirectToAction("EditEGears", new { id = model.eGearID });
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditEGears", new { id = model.eGearID });
            }
        }

        [HttpPost]
        public ActionResult DeleteEGear(ED_EGear model)
        {
            try
            {


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var egear = _entity.ED_EGear.First(m => m.eGearID == model.eGearID);
                    egear.Deleted = true;

                    _entity.SaveChanges();

                    TempData["Msg"] = "Record Deleted successfully.";
                    return RedirectToAction("EGears");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                return RedirectToAction("EditEGears", new { id = model.eGearID });
            }
        }


        #endregion

        #region MessageTemplates


        public ActionResult MessageTemplates()
        {
            ViewData["Msg"] = TempData["Msg"];

            var lstMsgTemp = _emailTemplateService.GetAll();
            
			return View(lstMsgTemp);
        }

        public ActionResult AddMessageTemplate()
        {

            ViewData["AllowedTokens"] = MessageManager.GetListOfAllowedTokens();
            return View();

        }
        [HttpPost]
        public ActionResult AddMessageTemplate(EmailTemplate model)
        {
			int messageTemplateID = _emailTemplateService.Save(model);

            TempData["Msg"] = "Record added successfully.";
            return RedirectToAction("EditMessagaeTemplate", new { id = messageTemplateID });
        }

        public ActionResult EditMessagaeTemplate(Int32 id)
        {
            ViewData["Msg"] = TempData["Msg"];
            ViewData["AllowedTokens"] = MessageManager.GetListOfAllowedTokens();
            var msgTempId = id;
            var msgTemp = _emailTemplateService.GetByID(msgTempId);
            
            return View(msgTemp);
        }

        [HttpPost]
        public ActionResult EditMessagaeTemplate(EmailTemplate template)
        {
	        int messageTemplateID = 0;

            try
            {
	            messageTemplateID = _emailTemplateService.Save(template);
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                
				return EditMessagaeTemplate(messageTemplateID);
            }
            
			TempData["Msg"] = "Record Updated successfully.";
            
			return RedirectToAction("EditMessagaeTemplate", new { id = messageTemplateID });
        }

        [HttpPost]
        public ActionResult DeleteMessagaeTemplate(_MessageTemplateLocalized model)
        {
            try
            {
	            _emailTemplateService.Delete(model.MessageTemplateID);
				
				TempData["Msg"] = "Record deleted successfully.";
                
				return RedirectToAction("MessageTemplates");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error:" + ex.Message;
                
				return RedirectToAction("EditMessagaeTemplate", new { id = model.MessageTemplateID });
            }

        }
        #endregion

    }
}
