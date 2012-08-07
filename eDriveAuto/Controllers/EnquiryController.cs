using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;


namespace Edrive.Controllers
{
    public class EnquiryController : Controller
    {
        //
        // GET: /Enquiry/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(_Enquiry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    string email = model.Email.Trim();
                    string fullName = model.FullName.Trim();
                    string subject = string.Format("{0}. {1}", SettingManager.StoreName, "Contact us");
                    string body = MessageManager.FormatContactUsFormText(model.Enquiry);

                    var from = new MailAddress(email, fullName);

                    //required for some SMTP servers
                    if (Convert.ToBoolean(SettingManager.GetSettingValue("Email.UseSystemEmailForContactUsForm")))
                    {
                        from = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                        body = string.Format("<b>From</b>: {0} - {1}<br /><br />{2}", Server.HtmlEncode(fullName), Server.HtmlEncode(email), body);
                    }
                    var to = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                    MessageManager.InsertQueuedEmail( from, to, string.Empty, string.Empty, subject, body, 0);

                    //pnlResult.Visible = true;
                    //lblMsg.Visible = false;
                    //pnlContactUs.Visible = false;
                        ViewData["Msg"]="Thank you, a representative will be in touch with you soon.";
                }
                catch (Exception exc)
                {
                    ViewData["Msg"]=exc.Message;

                    //LogManager.InsertLog(LogTypeEnum.MailError, string.Format("Error sending \"Contact us\" email."), exc);
                }
            }
        

            return View();
        }

    }
}
