using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class is used while register Dealers from front end
    /// </summary>
    public class _DealerRegisteration
    {
          [Required]
        public String Firstname { get; set; }
          [Required]
          public String Lastname { get; set; }
          [Required]
          public String Title { get; set; }
          [Required]
          public String Dealership { get; set; }
          [Required]
          public Int32 Zip { get; set; }
          [Required]
          public String Telephone { get; set; }
          public String StateID { get; set; }
          
          public String TelephoneExtenstion { get; set; }
          [Required]
          [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
          public String Email { get; set; }





          public String DMSProvider { get; set; }
    }
}