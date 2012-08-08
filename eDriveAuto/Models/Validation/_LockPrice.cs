using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _LockPrice
    {
        [Required]
        public String Name { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]

        public String Email { get; set; }
        [Required]
        public Int32 Phone { get; set; }
        [Required]
        public Int32 ProductID { get; set; }

        //public String DealerEmail { get; set; }

        public String vin { get; set; }
        public String ModelName { get; set; }   public String MakeName { get; set; }  

        public  decimal price_Current { get; set; }

        public  Int32? zip { get; set; }
        public String DealerName { get; set; }

        public Int32 Year { get; set; }

        public String Stock { get; set; }

        public String Type { get; set; }

        public Int32 Mileage { get; set; }

        public Boolean Warranty { get; set; }

        public String Transmission { get; set; }

        public String Interior { get; set; }

        public String Exterior { get; set; }

        public String companyName { get; set; }

        public String DealerPhone { get; set; }

        public String dealerCity { get; set; }

        public String streetAddress { get; set; }


        public String Engine { get; set; }
    }
}