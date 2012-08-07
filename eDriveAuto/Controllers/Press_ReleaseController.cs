using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class Press_ReleaseController : Controller
    {
        #region Variables
        public Int32 pageSize=10;
        #endregion
        #region Actions
        //
        // GET: /Press_Release/

        /// <summary>
        /// show the latest news
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                
                 try
                 {
                     ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Press-releases.metatitle");
                     ViewData["description"] = SettingManager.GetSettingValue("SEO.Press-releases.description");
                     ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Press-releases.keywords");

                
                 }
                 catch (Exception)
                 {

                 }

                var lstModel = _entity.Nop_News.Where(m => m.Published == true).OrderByDescending(m => m.NewsID).Take(pageSize).ToList();

                 ViewData["TotalCounts"]  = _entity.Nop_News.Where(m => m.Published == true).OrderByDescending(m => m.NewsID).Count();
                 ViewData["PageIndex"] = 0;
                 ViewData["PageSize"] = 10;
               return View(lstModel);
            }

        }
    [HttpPost]
        public ActionResult Index(String PageIndex, Int32? id)
        {

            var NewpageIndex = 0;
            if (String.IsNullOrEmpty(PageIndex) == false)
            {
                NewpageIndex = Convert.ToInt32(PageIndex);
            }

       // NewpageIndex = NewpageIndex == 0 ? 1 : NewpageIndex;//------index should be greater than 0
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                var lstModel = _entity.Nop_News.Where(m => m.Published == true).OrderByDescending(m => m.NewsID).Skip((NewpageIndex)*pageSize).Take(pageSize).ToList();

                ViewData["TotalCounts"] = _entity.Nop_News.Where(m => m.Published == true).OrderByDescending(m => m.NewsID).Count();
                ViewData["PageIndex"] = NewpageIndex;
                ViewData["PageSize"] = pageSize;
                return View(lstModel);
            }

        }
        /// <summary>
        /// To show the news detials
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewsDetails(String id)
        {
            try
            {
            var newsID =Convert.ToInt32( id.Substring(0, id.IndexOf('_')));
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var lstModel = _entity.Nop_News.First(m => m.NewsID == newsID);
                return View(lstModel);
            }
            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "Error: " + ex.Message;
                return View();
            }

        }


        [ChildActionOnly]
        public ActionResult RecentNews()
        {
            try
            {

           
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var lstModel = _entity.Nop_News.Where(m => m.Published == true).OrderByDescending(m => m.NewsID).Take(2).ToList();
                return PartialView(lstModel);
            }

            }
            catch (Exception)
            {

                return PartialView( null);
            }
        }
        #endregion


    }
}
