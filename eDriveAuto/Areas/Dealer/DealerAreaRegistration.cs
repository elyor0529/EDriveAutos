using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Dealer
{
    public class DealerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dealer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Dealer_default",
                "Dealer/{controller}/{action}/{id}",
                 new { action = "Index", controller = "DealerDashboard", id = UrlParameter.Optional} ,
                     new[] { "Edrive.Areas.Dealer.Controllers"  }
                
            );
        }
    }
}
