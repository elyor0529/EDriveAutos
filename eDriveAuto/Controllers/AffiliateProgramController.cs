using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class AffiliateProgramController : Controller
    {
        //
        // GET: /AffiliateProgram/

        public ActionResult Index()
        {
            try
            {
                ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-metatitle");
                ViewData["description"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-description");
                ViewData["keywords"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-keywords");


            }
            catch (Exception)
            {

            }

 
            return View();
        }
        
     

    }
}
