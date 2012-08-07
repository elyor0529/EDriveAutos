using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class is used in ManageContent in Admin section for Add/Edit EGears Records
    /// </summary>
    public class _EGear
    {
        public int eGearID { get; set; }
        [Required(ErrorMessage="*")]
        public String ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Qty { get; set; }
        public int ImageID { get; set; }
        [Required]
        public String ShortDesc { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public Boolean Published { get; set; }
        public Boolean Deleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    [MetadataType(typeof(_EGear))]
    public partial class ED_EGear
    {
    }
}