using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
    public static class PictureManager
    {
        public static Nop_Picture SavePicture(HttpPostedFileBase Pic  )
        {
            try
            {

            
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                #region "SaveFile"
                if (Pic!= null)
                    if (Pic.ContentLength > 0)
                    {
                        var Length = Pic.ContentLength;
                        byte[] filsbyte = new byte[Length];
                     Pic.InputStream.Read(filsbyte, 0, Length);
                        var ext = Path.GetExtension(Pic.FileName);
                        if (ext.Contains("."))
                            ext = ext.Substring(ext.LastIndexOf('.') + 1);
                        Nop_Picture newPartenerImage = new Nop_Picture { PictureBinary = filsbyte, IsNew = true, Extension = "image/" + ext };
                        _entity.Nop_Picture.AddObject(newPartenerImage);
                        _entity.SaveChanges();
                        return newPartenerImage;
                    }
                #endregion
            }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }
    }
}