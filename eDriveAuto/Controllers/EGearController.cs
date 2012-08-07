using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class EGearController : Controller
    {
		private readonly IStateProvinceService _stateProvinceService;
		private readonly ICountryService _countryService;

		public EGearController(IStateProvinceService stateProvinceService, ICountryService countryService)
		{
			_stateProvinceService = stateProvinceService;
			_countryService = countryService;
		}

        public ActionResult Index()
        {
            var lstCountry = _countryService.GetAll().OrderBy(m => m.DisplayOrder);
            ViewData["Countries"] = new SelectList(lstCountry, "CountryID", "Name");
            var states = _stateProvinceService.GetStateByCountry(lstCountry.First().CountryID);
                
			if (states != null)
            {
                ViewData["States"] = new SelectList(states, "StateProvinceID", "Name");
            }

            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                var lst = entity.ED_EGear.Where(m => m.Deleted == false).OrderBy(m => m.DisplayOrder).ToList();
                return View(lst);
            }
        }
    }
}
