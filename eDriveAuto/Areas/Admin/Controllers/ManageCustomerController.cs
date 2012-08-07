using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;
using Customer = Edrive.Models.Customer;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageCustomerController : Controller
    {
        // GET: /Admin/ManageCustomer/
        public int PageSize = 50;
        public ActionResult Index()
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var lst = service.GetDealers().Select(m => new { CompanyName = m.CompanyName }).Distinct().ToList();
                ViewData["Dealers"] = new SelectList(lst, "CompanyName", "CompanyName");
            }
            ViewData["IsPostBack"] = true;
            ViewData["Msg"] = TempData["Msg"];
            return View();
        }

        [HttpPost]
        public ActionResult Index(String AddNew, _CustomerManage Model_Search, Int32 PageIndex, String CustomerID, String SendMail)
        {

            if (AddNew != null)
            {
                return RedirectToAction("Add");
            }
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var lst = service.GetDealers().Select(m => new { CompanyName = m.CompanyName }).Distinct().ToList();
                ViewData["Dealers"] = new SelectList(lst, "CompanyName", "CompanyName");
            }

            using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
            {
                String expression = BuildExpression(Model_Search);
                ViewData["CarsCount"] = _entity.Customer.Where(expression).Count();
                var lstCustomer = _entity.Customer.Where(expression).OrderByDescending(m => m.CustomerID).Skip(PageSize * PageIndex).Take(PageSize).ToList();
                ViewData["Customers"] = lstCustomer;
                ViewData["PageIndex"] = PageIndex;
                return View();
            }
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(_CustomerInfo Model)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                Customer _cust = new Models.Customer();
                _cust.Email = Model.Email;
                _cust.Password = Model.Password;
                _cust.FirstName = Model.FirstName;
                _cust.LastName = Model.LastName;
                _cust.zip = Model.zip;
                _cust.Phone = Model.Phone;
                _cust.IsNewsLetter = Model.IsNewsLetter;
                _cust.RegisterationDate = DateTime.UtcNow;
                _cust.Active = true;
                _cust.Deleted = false;
                _cust.ExpirationDate = DateTime.Now.AddYears(1);
                _cust.CustomerType = _entity.Customer_Type.First(m => m.RoleName == "Customer").id;
                _entity.Customer.AddObject(_cust);
                _entity.SaveChanges();
                TempData["Msg"] = "Record Added Successfully.";
                return RedirectToAction("Edit", new { id = _cust.CustomerID });
            }
        }
        /// <summary>
        /// to update the Customer Password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="txtPassword"></param>
        /// <returns></returns>
       public ActionResult   ChangePassword(Int32 id,String txtPassword)
       {
           var Customerid = id;
           if(String.IsNullOrEmpty(txtPassword))
           {
               TempData["Msg"]="Password is required.";
               return RedirectToAction("Edit",new{id=id});
           }

           using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                
            Customer _custChangepassword = _entity.Customer.First(m => m.CustomerID == Customerid);
            _custChangepassword.Password = txtPassword;
            _entity.SaveChanges();
            }
            TempData["Msg"]="Password Updated successfully";
               return RedirectToAction("Edit",new{id=id});

       }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Int32 id)
        {
            ViewData["Msg"] = TempData["Msg"];
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                _CustomerInfo Model = null;
                try
                {
                    Customer _cust = _entity.Customer.FirstOrDefault(m => m.CustomerID == id && m.Deleted == false && (m.Customer_Type.RoleName == "Customer" || m.Customer_Type.RoleName == "Guest"));

                    Model = new _CustomerInfo();
                    if (_cust != null)
                    {
                        Model.Email = _cust.Email;
                        Model.Password = _cust.Password; 

                        Model.FirstName = _cust.FirstName;
                        Model.LastName = _cust.LastName;
                        //if (String.IsNullOrEmpty(Model.FirstName) && String.IsNullOrEmpty(Model.LastName))
                        //{
 
                        //}
                        Model.zip = _cust.zip ?? 0;
                        Model.Phone = _cust.Phone;
                        Model.IsNewsLetter = _cust.IsNewsLetter;
                        Model.CustomerID = _cust.CustomerID;
                        Model.RegisterationDate = _cust.RegisterationDate;
                    }
                    else
                    {
                        ViewData["Msg"] = "Customer not found";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Msg"] = ex.Message;
                }
                return View(Model);
            }

        }

        [HttpPost]
        public ActionResult Delete(String Save, String Delete, _CustomerInfo Model, String ChangePassword)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                Models.Customer _cust = _entity.Customer.FirstOrDefault(m => m.CustomerID == Model.CustomerID);
                _cust.Deleted = true;
                _entity.SaveChanges();
                TempData["Msg"] = "Record Deleted Successfully.";
                return RedirectToAction("Index");
            }
        }

        
        /// <summary>
        /// This method is for editing and deleting the User
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(String Save, String Delete, _CustomerInfo Model, String ChangePassword)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                if (Delete == "true")
                {
                    Models.Customer _cust = _entity.Customer.FirstOrDefault(m => m.CustomerID == Model.CustomerID);
                    _cust.Deleted = true;
                    _entity.SaveChanges();
                    TempData["Msg"] = "Record Deleted Successfully.";
                    return RedirectToAction("Index");
                }
                else                 {
                    //--save customer--
                    Models.Customer _cust = _entity.Customer.FirstOrDefault(m => m.CustomerID == Model.CustomerID);
                    _cust.Email = Model.Email;
                   // _cust.Password = Model.Password;
                    _cust.FirstName = Model.FirstName;
                    _cust.LastName = Model.LastName;
                    _cust.zip = Model.zip;
                    _cust.Phone = Model.Phone;
                    _cust.IsNewsLetter = Model.IsNewsLetter;
                    _entity.SaveChanges();
                    ViewData["Msg"] = "Record Updated Successfully.";
                    return View(Model);
                }
            }
        }
        /// <summary>
        /// Expression for search
        /// </summary>
        /// <param name="Model_Search"></param>
        /// <returns></returns>
        private static String BuildExpression(_CustomerManage Model_Search)
        {
            String expression = "it.Deleted == false and (it.Customer_Type.RoleName == 'Customer' or it.Customer_Type.RoleName == 'Guest' ) and true  ";
            if (!String.IsNullOrEmpty(Model_Search.CompanyName))
            {
                expression += String.Format(" and false ");
            }
            if (!String.IsNullOrEmpty(Model_Search.Email))
            {
                expression += String.Format(" and it.Email='{0}'", Model_Search.Email);
            }
            if (!String.IsNullOrEmpty(Model_Search.LastName))
            {
                expression += String.Format(" and it.LastName='{0}'", Model_Search.LastName);
            }
            //if (!String.IsNullOrEmpty(Model_Search.Name))
            //{
            //    expression += String.Format(" and it.Name='{0}'", Model_Search.Name);

            //}

            if ((Model_Search.RegFrom != null))
            {
                expression += String.Format(" and it.RegisterationDate>=DATETIME'{0}'", Model_Search.RegFrom.Value.ToString("yyyy-MM-dd hh:mm")); //'2010-13-8 00:00
            }
            if ((Model_Search.RegTo != null))
            {
                expression += String.Format(" and it.RegisterationDate<=DATETIME'{0}'", Model_Search.RegTo.Value.ToString("yyyy-MM-dd hh:mm"));
            }
            return expression;
        }

        public JsonResult IsCustomerExist(String Email, String CustomerID)
        {

          
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                //----new user adding--
                if (CustomerID == "0")
                {
                    if (_entity.Customer.Any(m => m.Deleted == false && m.Customer_Type.RoleName == "Customer" && m.Email == Email))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //---------
                //----edit user--
                else
                {
                    var _custID = Convert.ToInt32(CustomerID);
                    if (_entity.Customer.Any(m => m.Deleted == false && m.CustomerID != _custID && m.Customer_Type.RoleName == "Customer" && m.Email == Email))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
