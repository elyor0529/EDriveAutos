using System.Web.Mvc;
using $rootnamespace$.Filters;

namespace $rootnamespace$.Controllers
{
    public class HomeController : Controller
    {
	    [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
