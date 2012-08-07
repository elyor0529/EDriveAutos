using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _ED_Faq
    {
        public int FaqId { get; set; }
        public int OrderId { get; set; }

  public string Question { get; set; }
        [System.Web.Mvc.AllowHtml]
  public string Answer { get; set; }
  public Boolean IsActive { get; set; }
  public DateTime CreatedOn { get; set; }

  public DateTime UpdatedOn { get; set; }
   

    }

    [MetadataType(typeof(_ED_Faq))]
    public partial    class ED_Faq
    {

    }
    [MetadataType(typeof(_ED_Faq))]
    public partial class ED_DealersFaq
    {

    }
    

}