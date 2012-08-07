using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class Frequently_asked_questionsController : Controller
    {
        //
        // GET: /Frequently_asked_questions/

        public ActionResult Index()
        {
            try
             {
                try{
                    ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Frequently-asked-questions.metatitle");
                    ViewData["description"] = SettingManager.GetSettingValue("SEO.Frequently-asked-questions.description");
                    ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Frequently-asked-questions.keywords");

                }
                catch (Exception)
                {

                }


                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var lstUserFaqs = _entity.ED_Faq.Where(m => m.IsActive).OrderBy(m => m.OrderId).ToList();

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
