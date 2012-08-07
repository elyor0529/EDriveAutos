using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{

    public class _NopCampaign
    {
        public int CampaignID { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [AllowHtml]
        public string Body { get; set; }
        public string CreatedOn { get; set; }

      
    }
    [MetadataType(typeof(_NopCampaign))]
    public partial class Nop_Campaign
    {

    }
}