using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class PartnersController : Controller
    {
        //
        // GET: /Dealers/

        public ActionResult Index()
        {
            ViewData["Msg"] = TempData["Msg"];

            using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
            {

                try
                {
                    ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Partners.metatitle");
                    ViewData["description"] = SettingManager.GetSettingValue("SEO.Partners.description");
                    ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Partners.keywords");


                }
                catch (Exception)
                {

                }

 
                var lstImage = _enitity.ED_Partners.Where(m => m.IsActive == true && m.IsApproved == true).OrderByDescending(m=>m.PartnerId).Select(m => m.PictureId).ToList();
                if(lstImage.Count>0)
                ViewData["lstParteners"] = lstImage;

            }

            return View();
        }
        /// <summary>
        /// To show Image of size 600 480
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowMediumSizeImage(int? id)
        {
            if (id == null)
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            if (id == 0)
            {
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            }
            else
            {

                using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
                {
                    var pic = _enitity.Nop_Picture.FirstOrDefault(m => m.PictureID == id);
                    if (pic != null)
                    {
                        MemoryStream ms=new MemoryStream ();
                        ms.Write(pic.PictureBinary,0,pic.PictureBinary.Length);
                        var imageData = Bitmap.FromStream(ms);
                        Size newSize=imageData.Size;
                        if(imageData.Size.Width>600)
                        {
                            newSize.Width=600;
                        }
                        
                        if(imageData.Size.Height>480)
                        {
                            newSize.Height=480;
                        }
                        

                        Bitmap output = new Bitmap(imageData, newSize);
                        MemoryStream ms2=new MemoryStream ();
                        output.Save(ms2,ImageFormat.Jpeg);
                      
                        if (String.IsNullOrEmpty(pic.Extension))
                            return File(ms2.ToArray(), "image/jpeg");
                        else
                            return File(ms2.ToArray(), pic.Extension);
                    }
                    else
                    {
                        return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");
                    }

                }


            }
        }

        /// <summary>
        /// It return the image  with specifed width  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowCustomSizeImage(int? id,Int32 width)
        {
            if (id == null)
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            if (id == 0)
            {
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            }
            else
            {

                using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
                {
                    var pic = _enitity.Nop_Picture.FirstOrDefault(m => m.PictureID == id);
                    if (pic != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        ms.Write(pic.PictureBinary, 0, pic.PictureBinary.Length);
                        var imageData = Bitmap.FromStream(ms);
                        Size newSize = imageData.Size;
                        if (imageData.Size.Width > width)
                        {
                            newSize.Width = width;
                            newSize.Height=Convert.ToInt32( Math.Ceiling(imageData.Size.Height*((double)width/imageData.Width)));
                        }


                        Bitmap output = new Bitmap(imageData, newSize);
                        MemoryStream ms2 = new MemoryStream();
                        output.Save(ms2, ImageFormat.Jpeg);

                        if (String.IsNullOrEmpty(pic.Extension))
                            return File(ms2.ToArray(), "image/jpeg");
                        else
                            return File(ms2.ToArray(), pic.Extension);
                    }
                    else
                    {
                        return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");
                    }

                }


            }
        }
       
        /// <summary>
        /// to get the Image from database, this method is used by other controller to get the Image as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Show(int? id)
        {
            if(id==null)
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            if (id == 0)
            {
                return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");

            }
            else
            {
                 
                    using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
                    {
                        var pic = _enitity.Nop_Picture.FirstOrDefault(m => m.PictureID == id);
                        if (pic != null)
                        {
                            var imageData = pic.PictureBinary;
                            if (String.IsNullOrEmpty(pic.Extension))
                                return File(imageData, "image/jpeg");
                            else
                                return File(imageData, pic.Extension);
                        }
                        else
                        {
                            return File(Server.MapPath("~/Content/themes/App_Themes/EDrive/images/photo-comming-soon.jpg"), "image/jpg");
                        }

                    }
                 
                 
            }
        }
        
        [HttpPost]
        public ActionResult Index(ED_Partners _Partener)
        {
            using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
            {
                _Partener.IsApproved = false;
                _Partener.IsActive = true;
                _Partener.CreatedOn = DateTime.UtcNow;
                _Partener.UpdatedOn = DateTime.UtcNow;
                _Partener.PictureId = 0;
                _enitity.ED_Partners.AddObject(_Partener);
                
                _enitity.SaveChanges();

                TempData["Msg"] = "Thank you for writing to us. One of our representatives will contact you shortly.";
                #region Send Email to Admin
                string[] strEnquiry = new string[10];
                strEnquiry[0] = _Partener.Email;
                strEnquiry[1] = _Partener.FirstName;
                strEnquiry[2] = _Partener.LastName;
                if (_Partener.Phone != null)
                {
                    if (_Partener.Phone.Length > 0)
                    {
                        strEnquiry[3] = _Partener.Phone;
                    }
                    else
                    {
                        strEnquiry[3] = "---";
                    }
                }
                strEnquiry[4] = _Partener.Company;
                strEnquiry[5] = _Partener.Website;
                strEnquiry[6] = _Partener.Comments;
                MessageManager.SendPartnersToAdmin(strEnquiry, 0);

                #endregion
                return RedirectToAction("Index");
            }
        }
    }
}
