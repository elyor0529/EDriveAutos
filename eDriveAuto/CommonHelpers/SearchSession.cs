using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
	public enum SearchType
	{
		HomePageSearch = 0,
		AdvanceSearch = 1,
		SearchonSearchPage = 2,
		Cheap = 3,
		Luxury = 4,
		LeaseToOwn = 5
	}
	public class SearchSession
	{
		SearchType SearchTyped;
		HomePageSearch hmSearchParameter;
		AdvanceSearch advSearchParameter;
		SearchOnSearchPage SearchonSearchPageParameter;

		public SearchSession(SearchType searchType, String Searchkey, string SearchbyDealer, string ZipCode)
		{
			SearchTyped = searchType;
			hmSearchParameter = new HomePageSearch { SearchByDealer = SearchbyDealer, SearchKey = Searchkey, ZipCode = ZipCode };
		}
		public SearchSession()
		{

		}

		public SearchType prpSearchType
		{
			get
			{
				return SearchTyped;
			}
			set
			{
				SearchTyped = value;
			}
		}

		public SearchOnSearchPage prpSearchonSearchPageParameter
		{
			get
			{
				return SearchonSearchPageParameter;
			}
			set
			{
				SearchonSearchPageParameter = value;
			}

		}

		public AdvanceSearch prpAdvSearchParameter
		{
			get
			{
				return advSearchParameter;
			}
			set
			{
				advSearchParameter = value;
			}

		}
		public HomePageSearch prpHomePageSearch
		{
			get
			{
				return hmSearchParameter;
			}

		}

		public Object SearchValue
		{
			get
			{
				//HttpContext.Current.Session["Search"]==null)
				return HttpContext.Current.Session["Search"];
			}
			set
			{
				HttpContext.Current.Session["Search"] = value;

			}
		}

	}

	public class SearchOnSearchPage
	{
		public String Price { get; set; }
		public String Mileage { get; set; }
		public String Make { get; set; }
		public String Model { get; set; }
		public String Year { get; set; }
		public String Vin { get; set; }
		public String DriveType { get; set; }
		public String Transmission { get; set; }
		public String Engine { get; set; }
		public String Body { get; set; }
		public String Type { get; set; }
		public String Zip { get; set; }
		public String Warranty { get; set; }
		public String sortByColumn { get; set; }
		public int Radius { get; set; }

		public String pageSize { get; set; }
		public String PageIndex { get; set; }
		public String hiddenSearchKey { get; set; }
		public Int32 SearchByDealerID { get; set; }

	}
	public class AdvanceSearch
	{
		public String Body { get; set; }
		public String DriveType { get; set; }
		public String Engine { get; set; }
		public String PriceMax { get; set; }
		public String YearMax { get; set; }
		public String Mileage { get; set; }

		public String PriceMin { get; set; }
		public String YearMin { get; set; }
		public Int32 Make { get; set; }
		public Int32? Model { get; set; }
		public Int32 Radius { get; set; }
		public String Transmission { get; set; }

		public Int32 Type { get; set; }
		public String Vin { get; set; }

		public String Zip { get; set; }

	}

	public class HomePageSearch
	{
		public String SearchKey { get; set; }
		public String SearchByDealer { get; set; }
		public String ZipCode { get; set; }
	}
}