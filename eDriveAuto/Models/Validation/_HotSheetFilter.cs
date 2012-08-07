using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{ /// <summary>
    /// This class is created for _HotSheet Search Filter
    /// </summary>
    public class _HotSheetFilter
    {
        public String Miles { get; set; }
        [StringLength(9,ErrorMessage= "Maximum 9 Character are allowed.")]

        [Display(Name="Zip")]
        public Int32? ZipValue { get; set; }
        public Int32? Zip { get; set; }
        public Int32 MakeID { get; set;}
       

    }
}