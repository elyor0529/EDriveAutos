using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Edrive.Models
{
    public class _MessageTemplateLocalized
    {
        public int MessageTemplateLocalizedID { get; set; }
        public int MessageTemplateID { get; set; }
        [Required]
        public string Subject { get; set; }
        [System.Web.Mvc.AllowHtml]
             public string Body { get; set; }
        [Required]
        [Remote("isTemplateExists", "DashBoard", "Admin", AdditionalFields = "MessageTemplateID", ErrorMessage = "This template name already exists.")]
        public string TemplateName { get; set; }
        
   
        public string BCCEmailAddresses { get; set; }
        public Boolean IsActive { get; set; }

    }
    //[MetadataType(typeof(_MessageTemplateLocalized))]
    public partial class MessageTemplateLocalized
    {

    }

}