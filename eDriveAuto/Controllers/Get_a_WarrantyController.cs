using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class Get_a_WarrantyController : Controller
    {
        //
        // GET: /Get_a_Warranty/

        public ActionResult Index()
        {
            try
            {
                ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Geta-warranty.metatitle");
                ViewData["description"] = SettingManager.GetSettingValue("SEO.Get-a-warranty.description");
                ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Get-a-warranty.keywords");


            }
            catch (Exception)
            {

            }

 
            return View();
        }

    }
}
