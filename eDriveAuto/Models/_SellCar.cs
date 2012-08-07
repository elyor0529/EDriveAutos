using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _SellCar
    {
        [Required(ErrorMessage=" ")]
        [Remote("IsProductExist","Common",ErrorMessage="This VIN already Exist.")]
        public string VIN{ get; set; }
        [Required]
        public Int32 Mileage { get; set; }
        [Required(ErrorMessage=" ")]
       //[StringLength(5,MinimumLength=5,ErrorMessage="Wrong Zip")]
        public Int32 Zip { get; set; }
        
        [Required]
        public String Condition { get; set; }

    }
}