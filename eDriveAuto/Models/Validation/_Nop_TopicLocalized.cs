using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    /// <summary>
    /// This class used as model on  Edit topic pages
    /// </summary>
    public class _Nop_TopicLocalized
    {
      public Int32 TopicLocalizedID { get; set; }
        [Required]
          [DisplayName("Topic")]
      public String TopicName { get; set; }
      public Int32 TopicID { get; set; }
      public String Title { get; set; }

        [AllowHtml]
      public String Body { get; set; }
        [DisplayName("Created On")]
      public DateTime CreatedOn { get; set; }
          [DisplayName("Updated On")]
      public DateTime UpdatedOn { get; set; }
           [DisplayName("Meta Title")]

      public String MetaTitle { get; set; }
           [DisplayName("Meta keywords")]

           public String MetaKeywords { get; set; }
           [DisplayName("Meta description")]
           public String MetaDescription { get; set; }
    }
    //[MetadataType(typeof(_Nop_TopicLocalized))]
    //public partial class Nop_TopicLocalized
    //{


    //}
}