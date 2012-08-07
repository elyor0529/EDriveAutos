using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

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

        // public String Telephone { get; set; }




        //[Range(typeof(String), "true", "true", ErrorMessage = "you must acknowledge that you have read and agree to abide by the terms of use in order to register.")]
        public Boolean TermsofUse { get; set; }

        [Required(ErrorMessage = "*")]
        public String Address { get; set; }

        [Required(ErrorMessage = "*")]
        public String City { get; set; }

        [Required(ErrorMessage = "*")]
        public String State { get; set; }

        [Required(ErrorMessage = "*")]
        public String PaymentType { get; set; }


        // public string Coupon { get; set; }
        //[Required]
        //public String howDidYouHearAboutUs { get; set; }
        [Required(ErrorMessage = "*")]

        public string Country { get; set; }

        public String CreditCardNumber { get; set; }
        public String ExpMonth { get; set; }
        public String ExpYear { get; set; }

        public String CVV { get; set; }



    }
}