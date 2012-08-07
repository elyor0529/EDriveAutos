using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _ED_Testimonials
    {
        [Required]
        public String TContent { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Address { get; set; }
        // [RegularExpression(@"[^\s]+(\.(?i)(jpg|png|gif|bmp))$",ErrorMessage="Only file with extension .jpg,.jpeg,.png,.gif and .bmp  are allowed.")]
        //public String Picture { get; set; }

        public Int32  TId { get; set; }
        public Int32 PictureId{ get; set; }
        public  Boolean CreatedOn { get; set; }
        public  Boolean IsActive { get; set; }
        public  Boolean UpdatedOn { get; set; }


    }

    [MetadataType(typeof(_ED_Testimonials))]
    public partial class ED_Testimonials
    {
    }
}