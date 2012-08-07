using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _ChangePassword
    {
        [Required]
        public String  NewPassword { get; set; }
        
        [Required(ErrorMessage=" ")]
        [Compare("NewPassword",ErrorMessage="Confirm password must match")]
        public String ConfirmPassword { get; set; }

        [Required]
        public String CustomerEmail { get; set; }
        
        [Required]
        public String CustomerType{ get; set; }

        [Required]
        public String CustomerGUID{ get; set; }


    }
}