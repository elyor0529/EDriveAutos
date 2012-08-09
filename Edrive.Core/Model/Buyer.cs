using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Edrive.Core.Model
{
	[DataContract]
	public class Buyer
	{
		[DataMember]
		public int ID { get; set; }

		[DataMember]
		[Required]
		public string FirstName { get; set; }

		[DataMember]
		[Required]
		public string LastName { get; set; }

		[DataMember]
		[Required]
		[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
		public string Email { get; set; }

		[DataMember]
		public string Phone { get; set; }

		[DataMember]
		[DataType(DataType.Password)]
		[Compare("ConfirmPassword", ErrorMessage = "Confirm Password does not match")]
		public string Password { get; set; }

		[DataMember]
		public int? TypeID { get; set; }

		[DataMember]
		public DateTime RegistrationDate { get; set; }

		[DataMember]
		public DateTime? ExpirationDate { get; set; }

		[DataMember]
		public string IPAddress { get; set; }

		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public string City { get; set; }

		[DataMember]
		public string State { get; set; }

		[DataMember]
		[Required]
		public string Zip { get; set; }

		[DataMember]
		[Required]
		public bool IsActive { get; set; }

		[DataMember]
		[Required]
		public bool IsDeleted { get; set; }

		[DataMember]
		[Required]
		public bool IsNewsLetter { get; set; }

		[DataMember]
		public bool? IsTrial { get; set; }
	}
}