using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _PersonalDetails
    {
        public Int32 CustomerID { get; set; }
        [Required]
        public String  Name { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        public String Email { get; set; }
        [Required]
        public Int32 PostalCode { get; set; }
        [Required]
        
        public String Telephone { get; set; }


    }
}