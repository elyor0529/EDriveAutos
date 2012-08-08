using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class EGearController : Controller
    {
        //
        // GET: /EGear/

        public ActionResult Index()
        {
            using (Edrive_ServiceClient _servicae = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var lstCountry = _servicae.GetCountry().OrderBy(m => m.DisplayOrder);
                ViewData["Countries"] = new SelectList(lstCountry, "CountryID", "Name");
                var states = _servicae.GetStateByCountry(lstCountry.First().CountryID);
                if (states != null)
                {
                    ViewData["States"] = new SelectList(states, "StateProvinceID", "Name");
                }


               
            }
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var lst = _entity.ED_EGear.Where(m => m.Deleted == false).OrderBy(m => m.DisplayOrder).ToList();
                return View(lst);
            }

           
        }

    }
}
