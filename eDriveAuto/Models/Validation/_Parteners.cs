using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class is using on Parteners Pages n for validation
    /// </summary>
    public class _Parteners
    {

        public int PartnerId { get; set; }
        
        [Required(ErrorMessage="*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage="*")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage="*")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "*Wrong Email")]
        public string Email { get; set; }
        [Required(ErrorMessage="*")]
        public string Company { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]
        [Required(ErrorMessage="*")]
        public string Website { get; set; }
        public string Comments { get; set; }
        public int PictureId { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    [MetadataType(typeof(_Parteners))]
    public partial class ED_Partners
    {

    }
}