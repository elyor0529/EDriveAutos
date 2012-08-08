using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class is used in Contactus Form 
    /// </summary>
    public class _ContactUsForm
    {
        [Required]
        public String Name  { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
    public String  Email{ get; set; }
    public String CustomerType  { get; set; }
    public String Address { get; set; }
    public String HearAboutUsBy { get; set; }
    public String City { get; set; }
    public String Country { get; set; }
    public String State { get; set; }
    public String Zip { get; set; }
    public String Telephone { get; set; }
    public String TelephoneExt { get; set; }
    public String Fax { get; set; }
    public String UserRequest_Question { get; set; }  

    }
}