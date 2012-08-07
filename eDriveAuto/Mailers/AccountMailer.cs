using System;
using System.Net.Mail;
using Mvc.Mailer;
using Edrive.Models;

namespace Edrive.Mailers
{
    public class AccountMailers : MailerBase
    {
        public AccountMailers()
        {
            MasterName = "_Layout";
        }

        public virtual MailMessage WelcomeMember(WelcomeMemberModel model)
        {
            var mailMessage = new MailMessage { Subject = "Welcome" };

            mailMessage.To.Add(model.EmailAddress);
            PopulateBody(mailMessage, viewName: "WelcomeMember");

            return mailMessage;
        }

		public virtual MailMessage ForgotPassword(ForgotPasswordModel model)
        {
            var mailMessage = new MailMessage { Subject = "Welcome" };

            mailMessage.To.Add(model.EmailAddress);
            PopulateBody(mailMessage, viewName: "WelcomeMember");

            return mailMessage;
        }
    }
}