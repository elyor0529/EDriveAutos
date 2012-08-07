using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class VendorsController : Controller
    {
        //
        // GET: /Vendor_/

        public ActionResult Index()
        {
            return View();
        }

    }
}
