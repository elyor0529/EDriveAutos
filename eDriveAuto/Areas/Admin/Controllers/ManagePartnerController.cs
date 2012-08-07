using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize( Roles="Admin")]
    public class ManagePartnerController : Controller
    {
        //
        // GET: /Admin/ManagePartener/
        /// <summary>
        /// To show manage Section
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
               var lst= _entity.ED_Partners.OrderByDescending(m=>m.PartnerId).ToList();
               ViewData["Msg"] = TempData["Msg"];
                return View(lst);
            }
        }

        /// <summary>
        /// To show manage Section
        /// </summary>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult Index(String AddNew)
        {
            if (AddNew != null)
            {
                return RedirectToAction("Add");
            }
            return View();

        }
        

        /// <summary>
        /// To show edit details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Int32 id)
        {
            ViewData["Msg"] = TempData["Msg"];
            Int32 partenerid = id;
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                var lst = _entity.ED_Partners.First(m=>m.PartnerId==id);
                return View(lst);
            }
        }
        /// <summary>
        /// to edit the content parteners n save changes
        /// </summary>
        /// <param name="_Partener"></param>
        /// <param name="PartenerImage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ED_Partners _Partener, String PartenerImage)
        {
            using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
            {
                var partImageID=_Partener.PictureId;//partener image id
                #region "SaveFile"
                if (Request.Files["PartenerImage"] != null)

                    if (Request.Files["PartenerImage"].ContentLength >0)
                    {
                        var Length=Request.Files["PartenerImage"].ContentLength;
                        byte[] filsbyte = new byte[Length];
                        Request.Files["PartenerImage"].InputStream.Read(filsbyte, 0, Length);
                        var ext=Path.GetExtension(Request.Files["PartenerImage"].FileName);
                        if(ext.Contains("."))
                            ext=ext.Substring(ext.LastIndexOf('.')+1);
                        Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                        _enitity.Nop_Picture.AddObject(newPartenerImage);
                        _enitity.SaveChanges();
                        partImageID = newPartenerImage.PictureID;
                    }
 
                
                #endregion
               var upd_partener= _enitity.ED_Partners.First(m => m.PartnerId == _Partener.PartnerId);
               upd_partener.IsActive = _Partener.IsApproved ?? false;
               upd_partener.IsApproved = _Partener.IsApproved??false;
                upd_partener.UpdatedOn = DateTime.UtcNow;
                upd_partener.PictureId = partImageID;
                upd_partener.LastName = _Partener.LastName;
                upd_partener.FirstName = _Partener.LastName;
                upd_partener.Phone = _Partener.Phone;
                upd_partener.Website = _Partener.Website;
                upd_partener.Comments= _Partener.Comments;
                upd_partener.Company = _Partener.Company;
                upd_partener.Email = _Partener.Email;

                ViewData["Msg"] = "Record Updated successfully.";
                _Partener.PictureId = upd_partener.PictureId;
                _enitity.SaveChanges();

            }
            return View(_Partener);
        }
        /// <summary>
        /// To delete the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            Int32 partenerid = id;
            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
             var obj= _entity.ED_Partners.First(m => m.PartnerId == id);
             _entity.ED_Partners.DeleteObject(obj);
              _entity.SaveChanges();
             TempData["Msg"] = "Record Deleted successfully.";
             return RedirectToAction("Index");
    
            }
            
        }

       /// <summary>
       /// To add new Partner
       /// </summary>
       /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }
    
        /// <summary>
        /// Save the New Partner
        /// </summary>
        /// <param name="_Partener"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult Add(ED_Partners _Partener)
        {

            using (eDriveAutoWebEntities _enitity = new eDriveAutoWebEntities())
            {
                var partImageID = 0;//partener image id
               
                #region "SaveFile"
                if (Request.Files["PartenerImage"] != null)

                    if (Request.Files["PartenerImage"].ContentLength > 0)
                    {
                        var Length = Request.Files["PartenerImage"].ContentLength;
                        byte[] filsbyte = new byte[Length];
                        Request.Files["PartenerImage"].InputStream.Read(filsbyte, 0, Length);
                        var ext = Path.GetExtension(Request.Files["PartenerImage"].FileName);
                        if (ext.Contains("."))
                            ext = ext.Substring(ext.LastIndexOf('.') + 1);
                        Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                        _enitity.Nop_Picture.AddObject(newPartenerImage);
                        _enitity.SaveChanges();
                        partImageID = newPartenerImage.PictureID;
                    }


                #endregion
                
                _Partener.IsActive = true;
                _Partener.CreatedOn = DateTime.UtcNow;
                _Partener.UpdatedOn = DateTime.UtcNow;
                _Partener.PictureId = partImageID;
                _enitity.ED_Partners.AddObject(_Partener);
                _enitity.SaveChanges();
                TempData["Msg"] = "Record added successfully.";
                return RedirectToAction("Edit", new { id=_Partener.PartnerId});

            }
        }
    

    }
}
