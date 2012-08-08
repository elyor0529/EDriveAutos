using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class DisclaimerController : Controller
    {
        //
        // GET: /Disclaimer/

        public ActionResult Index()
        {
            return View();
        }

    }
}
