using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class UserLogin
    {
        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Wrong Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}