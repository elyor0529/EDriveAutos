using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

using ED_Testimonials = Edrive.Models.ED_Testimonials;

namespace Edrive.Controllers
{
    public class Customers_SpeakController : Controller
    {
        public Int32 PageSize = 25;
        //
        // GET: /Customers_Speak/

        public ActionResult Index()
        {
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                try
                {
                    ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Testimonials.metatitle");
                    ViewData["description"] = SettingManager.GetSettingValue("SEO.Testimonials.description");
                    ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Testimonials.Keywords");


                }
                catch (Exception)
                {

                }

 
                var lst = _entity.ED_Testimonials.Where(m => m.IsActive).OrderByDescending(m => m.CreatedOn).Take(PageSize).ToList();
              ViewData["TotalCounts"] = _entity.ED_Testimonials.Where(m => m.IsActive).OrderByDescending(m => m.CreatedOn).Count();
                ViewData["PageIndex"] = 0;
              ViewData["PageSize"] = PageSize;
                return View(lst);
            }
        }
         
        
        [HttpPost]
        public ActionResult Index(String PageIndex)
        {
            var NewpageIndex = 0;
            if (String.IsNullOrEmpty(PageIndex) == false)
            {
                NewpageIndex = Convert.ToInt32(PageIndex);
            }

            // NewpageIndex = NewpageIndex == 0 ? 1 : NewpageIndex;//------index should be greater than 0
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                var lstModel = _entity.ED_Testimonials.Where(m => m.IsActive == true).OrderByDescending(m => m.CreatedOn).Skip((NewpageIndex) * PageSize).Take(PageSize).ToList();
                ViewData["TotalCounts"] = _entity.ED_Testimonials.Where(m => m.IsActive == true).OrderByDescending(m => m.CreatedOn).Count();
                ViewData["PageIndex"] = NewpageIndex;
                ViewData["PageSize"] = PageSize;
                return View(lstModel);
            }

        }

        /// <summary>
        /// Save the changes testimonial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTestimonial(_ED_Testimonials model)
        {
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                ED_Testimonials newObj = new ED_Testimonials { Address = model.Address, CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now, Name = model.Name, TContent = model.TContent };
                newObj.IsActive = true;
                newObj.PictureId = 0;
                if(Request.Files["Pic"]!=null)
                {
                    if (Request.Files["Pic"].ContentLength > 0)
                    {

                    var newPicd=   PictureManager.SavePicture(Request.Files["Pic"]);
                    if (newPicd != null)
                    {
                        newObj.PictureId = newPicd.PictureID;
                    }
                    }
                
                }
                _entity.ED_Testimonials.AddObject(newObj);
                _entity.SaveChanges();
            }
            return RedirectToAction("Index");
        }
       /// <summary>
       /// To show the Latest Testimonial on Home page
       /// </summary>
       /// <returns></returns>
        [ChildActionOnly]
        public ActionResult LatestTestimonial()
        {
            try
            {

            
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                var lst = _entity.ED_Testimonials.Where(m => m.IsActive).OrderByDescending(m => m.UpdatedOn).Take(2).ToList();
                return PartialView(lst);

            }
            }
            catch (Exception ex)
            {
                return PartialView(null);
            }
        }
    
    }
}
