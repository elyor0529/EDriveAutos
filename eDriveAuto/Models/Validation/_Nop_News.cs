using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _Nop_News
    {
        public int NewsID { get; set; }
        public int LanguageID { get; set; }
    public string  Title { get; set; }

    public string  Short { get; set; }
    [AllowHtml]
public string  Full { get; set; }
public string  Published { get; set; }
public string  AllowComments { get; set; }
public DateTime  CreatedOn { get; set; }


    }
  [MetadataType(typeof(_Nop_News))]
    public partial class Nop_News
    { 
    }
}