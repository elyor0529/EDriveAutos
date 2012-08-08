using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        /// <summary>
        /// To check email exist for all customer types
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public JsonResult IsEmailExist(String Email)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                // if  Email exist for Customer whole role is not in Guest
                if (_entity.Customer.Any(m => m.Deleted == false && m.Email == Email && m.Customer_Type.RoleName!="Guest"))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                using (Edrivie_Service_Ref.Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    if (_service.IsDealerExists(Email))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);

                    }
                }
 
            }
            
        }
       
        /// <summary>
        /// to check email exist for customer in Manage Customer's Edit Customer sectin
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public JsonResult IsCustomerExist(String Email, String CustomerID)
        {

            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                //----new user adding--
                if (CustomerID == "0")
                {
                    return IsEmailExist(Email);
                }
                //---------
                //----edit user--
                else
                {
                    var _custID = Convert.ToInt32(CustomerID);
                    // if customer or guest exist
                    if (_entity.Customer.Any(m => m.Deleted == false && m.CustomerID != _custID && m.Email == Email))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else { // if dealer exist
                        using (Edrivie_Service_Ref.Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                        {
                            if (_service.Is_other_DealerExist_for_same_email(Email, _custID))
                            {
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                            else
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To check if product exist by its vin or not
        /// </summary>
        /// <param name="VIN"></param>
        /// <returns></returns>
        public JsonResult IsProductExist(string VIN)
        {
            try
            {

            
            using (Edrivie_Service_Ref.Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {
                if (_service.IsProductExist_by_VIN(VIN))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            }
            catch (Exception ex)
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// To get the WebSetting Values proveid the Setting Name
        /// </summary>
        /// <param name="SetttingName"></param>
        /// <returns></returns>
        public ContentResult GetSettings(String SetttingName)
        {

            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                var chaseImage = _entity.Settings.FirstOrDefault(m => m.Name == "Media.Images.Chase");
                if (chaseImage != null)
                {
                  return Content(chaseImage.Value);
                }
                else
                {
                    return Content("");
                }
            }
        }

    }
}
