using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
  
    public class IntrestedCustomer
    {
        [Required(ErrorMessage="First Name is required")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string lastname { get; set; }
        public int productId { get; set; }
        [Display(Name="Email")]
        //[Required(ErrorMessage = "*")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        public string email { get; set; }
        //[Required(ErrorMessage = "*")]
        [Display(Name="Phone number")]
        public string phoneno { get; set; }
        public int zipcode { get; set; }
        public string contactType { get; set; }
        public int intrestType { get; set; }
        public string additionalComments { get; set; }
        public bool IsActive { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updateOn { get; set; }
        public bool finacing { get; set; }
        public bool Trade_in { get; set; }
        public bool price_Lock { get; set; }
        public int customerId { get; set; }
    }
}