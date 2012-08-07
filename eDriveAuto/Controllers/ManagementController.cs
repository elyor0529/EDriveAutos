using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class ManagementController : Controller
    {
        //
        // GET: /Management/

        public ActionResult Index()
        {
            using (eDriveAutoWebEntities _entitye = new Models.eDriveAutoWebEntities())
            {
               var model= _entitye.ED_EManagement.Where(m => m.Deleted == false).OrderBy(m => m.DisplayOrder).ToList();
                return View(model);
            }
        }

    }
}
