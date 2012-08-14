using System;

namespace Edrive.Core.Model
{
	public class Testimonial
	{
		public int ID { get; set; }

		public string Content { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public int? PictureID { get; set; }

		public bool IsActive { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime? DateUpdated { get; set; }
	}
}