using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Admin.Controllers
{
    public class Contact_UsController : Controller
    {
        //
        // GET: /Admin/ContactUs/

        public ActionResult Index()
        {
            return View("Index");
        }

    }
}
