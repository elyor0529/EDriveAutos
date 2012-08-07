using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class How_it__worksController : Controller
    {
        //
        // GET: /How_it__works/

        public ActionResult Index()
        {
            try
            {

                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var lstUserFaqs = _entity.ED_DealersFaq.Where(m => m.IsActive).OrderBy(m => m.OrderId).ToList();

                    return View(lstUserFaqs);
                }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error:" + ex.Message;
                return View();

            }
        }

    }
}
