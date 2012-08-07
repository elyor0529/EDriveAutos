using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Dealer.Controllers
{
    public class Contact_UsController : Controller
    {
        //
        // GET: /Dealer/Contact_Us/

        public ActionResult Index()
        {
            return RedirectToAction("Index","ContactUs", new { area = "" });
        }

    }
}
