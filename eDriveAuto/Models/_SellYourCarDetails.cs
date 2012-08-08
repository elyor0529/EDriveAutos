using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _SellYourCarDetails
    {
        [Required]
        public String VIN { get; set; }
        [Required]
        public String  Name { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        public String Email { get; set; }
        [Required]
        public String Phone{ get; set; }
        
        public String Notes { get; set; }

        public Int32 Mileage { get; set; }
        public String Condition { get; set; } 
        public int zip { get; set; }
        public String Offer { get; set; }

    }
}