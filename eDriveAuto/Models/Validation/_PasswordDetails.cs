using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _PasswordDetails
    {
        [Required(ErrorMessage="*")]
        [DataType(DataType.Password)]
        public String OldPassword  { get; set; }
       [Required(ErrorMessage="*")]
        
        [DataType(DataType.Password)]
        public String NewPassword { get; set; }
     [Required(ErrorMessage="*")]
        
        [Compare("NewPassword",ErrorMessage="Confirm Password does not match.")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    
        public int CustomerID { get; set; }

    }
}