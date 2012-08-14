using System.Web.Mvc;
using Edrive.CommonHelpers;

namespace Edrive.Controllers
{
	public class AffiliateProgramController : Controller
	{
		public ActionResult Index()
		{
			ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-metatitle");
			ViewData["description"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-description");
			ViewData["keywords"] = SettingManager.GetSettingValue("SEO.AffiliateProgram-keywords");

			return View();
		}
	}
}
