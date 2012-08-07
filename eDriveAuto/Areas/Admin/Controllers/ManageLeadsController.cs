using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;
using IntrestedCustomer = Edrive.Edrivie_Service_Ref.IntrestedCustomer;

namespace Edrive.Areas.Admin.Controllers
{
    //[Authorize(Roles="Admin")]
    public class ManageLeadsController : Controller
    {
        //
        // GET: /Admin/ManageLeads/
        Int32 pageSize = 10;
        public ActionResult Index()
        {
            //if (User.IsInRole("Dealer"))
            //{
            //    return RedirectToAction("Index", "DealerDashboard", new { area = "Dealer" });
            //}
            return View();
        }

        [HttpPost]
        public ActionResult Index(String InfoType)
        {
            switch (InfoType)
            {
                case "View More Info Requests":return RedirectToAction("InterestedCar",new {id=1});
                case "View Financing Requests": return RedirectToAction("FinancingRequests");
				case "View Customer Vehicle Requests": return RedirectToAction("CustomerVehicleRequests");
            }
            return View();
        }

		public ActionResult CustomerVehicleRequests()
		{
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				ViewData["Msg"] = TempData["Msg"];
				var lstModel = entity.CustomerVehicleRequests.Include("Customer").Select(m => m).OrderByDescending(m => m.CreationDate).Skip(0).Take(pageSize).ToList();
				ViewData["CarsCount"] = entity.CustomerVehicleRequests.Count();// total records count
				ViewData["PageIndex"] = 0;
				ViewData["PageSize"] = pageSize;

				return View(lstModel);
			}
		}

		[HttpPost]
		public ActionResult CustomerVehicleRequests(int PageIndex)
		{
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				var lstModel = entity.CustomerVehicleRequests.Include("Customer").Select(m => m).OrderByDescending(m => m.CreationDate).Skip(pageSize * PageIndex).ToList();
				ViewData["CarsCount"] = entity.CustomerVehicleRequests.Count();
				ViewData["PageIndex"] = PageIndex;
				ViewData["PageSize"] = pageSize;
				
				return View(lstModel);
			}
		}

        public ActionResult FinancingRequests()
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                ViewData["Msg"] = TempData["Msg"];
                var lstModel = _entity.FinanceRequest.Select(m => m).OrderByDescending(m => m.CreatedOn).Skip(0).Take(pageSize).ToList();
               ViewData["CarsCount"] = _entity.FinanceRequest.Count();// total records count
               ViewData["PageIndex"] = 0;
               ViewData["PageSize"] = pageSize;
                return View(lstModel);
            }
        }

        [HttpPost]
        public ActionResult FinancingRequests(Int32 PageIndex)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {

                var lstModel = _entity.FinanceRequest.Select(m => m).OrderByDescending(m => m.CreatedOn).Skip(pageSize*PageIndex).ToList();
                ViewData["CarsCount"] = _entity.FinanceRequest.Count();
                ViewData["PageIndex"] = PageIndex;
                ViewData["PageSize"] = pageSize;
                return View(lstModel);
            }
        }
        [HttpPost]
        public ActionResult DeleteFinance(Int32 id)
        {
           using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                try
                {

                
               var finance= _entity.FinanceRequest.First(m => m.id == id);
               _entity.FinanceRequest.DeleteObject(finance);
               _entity.SaveChanges();
               TempData["Msg"] = "Record deleted successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Error=" + ex.Message;
                }
                return RedirectToAction("FinancingRequests");
           }
        }
        
        public ActionResult InterestedCar(Int32 id)
        {
           ViewData["Msg"]= TempData["Msg"];
            ViewData["InterestType"] = id;//id is the interesttype
            // interested type 
            // 1 for financing
            //2 for trade in info
            using (Edrivie_Service_Ref.Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
               var lstInterestedCustomer= service.Get_InterestedCustomer(id);
               return View(lstInterestedCustomer);
            }
        }
        public PartialViewResult interestedCustomerDetails(IntrestedCustomer model)
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                try
                {

                
               var prdt= service.GetProductByID(model.productId);
               ViewData["Stock"] = prdt.stock;
               ViewData["PrdtName"] = prdt.Year +  " " +prdt.MakeName +" " +prdt.ModelName+" "+ prdt.body;
               ViewData["Price"] = prdt.price_Current;
            return PartialView(model);
                }
                catch (Exception)
                {
                    return PartialView(model);
                }
       
            }
             }


        [HttpPost]
        public ActionResult DeleteInterestedCar(Int32 id,Int32 InterestType )
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                String Msg;
                if (service.Delete_InterestedCustomer(out Msg, id))
                {
                    TempData["Msg"] = "Record Deleted successfully.";
                }
                else
                {
                    TempData["Msg"] = "Error:"+Msg;
 
                }
                return RedirectToAction("InterestedCar", new { id = InterestType });
            }
 
        }


    }
}
