using System.Collections.Generic;
using Edrive.Logic.Interfaces;
using Country = Edrive.Core.Model.Country;
using System.Linq;

namespace Edrive.Logic
{
	public class CountryService : BaseService, ICountryService
	{
		public List<Country> GetAll()
		{
			using(Context = GetDataContext())
			{
				var countries = Context.LST_COUNTRY.OrderBy(m => m.DISPLAYORDER).Select(
					m => new Country
					     	{
					     		CountryID = m.ID,
					     		DisplayOrder = m.DISPLAYORDER,
					     		Name = m.NAME
					     	}).ToList();

				return countries;
			}
		}
	}
}