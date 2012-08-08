using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
      [MetadataType(typeof(_customer))]
    public  class  CustomerModel:Customer
    {
          [Required]
          [DataType(DataType.Password)]
          public string ConfirmPassword { get; set; }
    }
    /// <summary>
    /// This class is using for Product Details Section for User Registeration.
    /// </summary>
    [Bind(Include = "Email")]
      public class _customer
      {
          

          [Required]
          [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
          public string Email { get; set; }
          [Required]
          public string FirstName { get; set; }
          [Required]
          public string LastName { get; set; }
          [Required]
        [DataType(DataType.Password)]
          [Compare("ConfirmPassword", ErrorMessage = "Confirm Password does not match")]
          public string Password { get; set; }
          [Required]
          
          public string ConfirmPassword { get; set; }
          
          public string Phone { get; set; }
          [Required]
          public Int32 zip { get; set; }

      }    
     
    
}  