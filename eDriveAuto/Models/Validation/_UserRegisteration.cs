using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Edrive.Models
{
    /// <summary>
    /// This class is used while register users from front end
    /// </summary>
    public class _UserRegisteration
    {
        [Required(ErrorMessage = "*")]
        public String Firstname { get; set; }
        [Required(ErrorMessage = "*")]
        public String Lastname { get; set; }


        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("Password", ErrorMessage = "Passwords didn't match.")]
        public String ConfirmPassword { get; set; }

        [Remote("IsEmailExist", "Common", ErrorMessage = "This Email already in use.")]
        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        public String Email { get; set; }


        [Required(ErrorMessage = " ")]
        [Compare("Email", ErrorMessage = "Re-type Email must match")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        public String ConfimrEmail { get; set; }

        [Required(ErrorMessage = "*")]
        public Int32 Zip { get; set; }

		[Required(ErrorMessage = "*")]
		public string ZipCode { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		
		public string VehicleInterest { get; set; }

		public string MaxBudget { get; set; }

		[Display(Name = "I have a trade-in")]
		public bool HaveTradeIn { get; set; }

		[Display(Name = "I need cheap financing")]
		public bool NeedCheapFinancing { get; set; }

		[Display(Name = "I need cheap insurance")]
		public bool NeedCheapInsurance { get; set; }

		[Display(Name = "I am interested in becoming a Premimum Member ($29.95 Lifetime)")]
		public bool InterestedForPremiumMember { get; set; }

	    public Boolean TermsofUse { get; set; }

        [Required(ErrorMessage = "*")]
        public String Address { get; set; }

        [Required(ErrorMessage = "*")]
        public String City { get; set; }

        [Required(ErrorMessage = "*")]
        public String State { get; set; }

        [Required(ErrorMessage = "*")]
        public String PaymentType { get; set; }

        [Required(ErrorMessage = "*")]

        public string Country { get; set; }

        public String CreditCardNumber { get; set; }
        public String ExpMonth { get; set; }
        public String ExpYear { get; set; }

        public String CVV { get; set; }
    }
}