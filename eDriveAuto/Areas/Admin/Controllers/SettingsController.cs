using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        //
        // GET: /Admin/Settings/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ChaseLogo()
        {
            ViewData["Msg"] = ViewData["Msg"];
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                var chaseImage = _entity.Settings.FirstOrDefault(m => m.Name == "Media.Images.Chase");
                if(chaseImage!=null)
                {
                    ViewData["Chase"] = chaseImage.Value;
                }
            }
            return View();
 
        }
        [HttpPost]
        public ActionResult ChaseLogo(String fupLogotext)
        {
            try
            {

            
            if (Request.Files["fupLogo"]==null)
            {
                ViewData["Msg"] = "Error: no file is selectec to be uploaded";
                    return View();
             }

                
            
            else
            if (Request.Files["fupLogo"].ToString() ==String.Empty)
                {
                    ViewData["Msg"] = "Error: no file is selectec to be uploaded";
                    return View();
                }
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
               var chaseImage= _entity.Settings.FirstOrDefault(m => m.Name == "Media.Images.Chase");
               String filePath = "/Content/Images/Chase_" + DateTime.Now.Ticks.ToString() + Path.GetExtension(Request.Files["fupLogo"].FileName.ToString());

               if (chaseImage == null)
               {
                   chaseImage = new Models.Settings { Name = "Media.Images.Chase", Value = filePath };
                   _entity.Settings.AddObject(chaseImage);
               }
               else
               {
                   chaseImage.Value = filePath;
               }
                Request.Files["fupLogo"].SaveAs(Server.MapPath("~"+filePath));
                _entity.SaveChanges();


                TempData["Msg"] = "Updated successfully.";
                return RedirectToAction("ChaseLogo");
            }
            }
            catch (Exception ex)
            {

                ViewData["Msg"] = "Error: "+ex.Message;
                return View();
            }

        }
    }
}
