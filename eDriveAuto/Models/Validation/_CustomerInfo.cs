using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class is created for Mange Customer Section
    /// </summary>
    public class _CustomerInfo
    {
        public Int32 CustomerID { get; set; }

        [Required(ErrorMessage=" ")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        [MyRemote("IsCustomerExist", "Common", areaName: "", AdditionalFields = "CustomerID", ErrorMessage = "This Email already exists.")]
        public string Email { get; set; }
        
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[Compare("ConfirmPassword", ErrorMessage = "Confirm Password does not match")]
        public string Password { get; set; }

        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [DisplayName("Zip Code")]
        [Required]
        public Int32 zip { get; set; }
                 [DisplayName("Newsletter")]
        public Boolean IsNewsLetter { get; set; }

         [DisplayName("Registeration Date")]
        public DateTime RegisterationDate { get; set; }



    }
}