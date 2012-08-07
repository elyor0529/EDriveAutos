using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _ED_EManagement
    {
    [Required]
        public String Title { get; set; }
        public int ImageID { get; set; }
         [Required]
        public String ShortDesc { get; set; }
         [Required]
        public int DisplayOrder { get; set; }
        public Boolean Published { get; set; }
        public Boolean Deleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
 

    }
    [MetadataType(typeof(_ED_EManagement))]
    public partial class ED_EManagement
    {
    }

}