using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Edrive.Core.Model
{
	public class EmailTemplate
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Remote("isTemplateExists", "DashBoard", "Admin", AdditionalFields = "MessageTemplateID", ErrorMessage = "This template name already exists.")]
		public string Name { get; set; }
		
		public string Bcc { get; set; }

		[Required]
		public string Subject { get; set; }
		
		[AllowHtml]
		[Required]
		public string Body { get; set; }

		public bool? IsActive { get; set; } 
	}
}