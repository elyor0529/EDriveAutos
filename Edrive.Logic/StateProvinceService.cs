using System.Collections.Generic;
using Edrive.Core.Model;
using System.Linq;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class StateProvinceService : BaseService, IStateProvinceService
	{
		public State GetByID(int stateID)
		{
			using(Context = GetDataContext())
			{
				var state = Context.LST_STATE.Where(m => m.ID == stateID)
					.Select(m => new State
					{
						StateProvinceID = m.ID,
						Abbreviation = m.ABBREVIATION,
						CountryID = m.COUNTRY_ID,
						DisplayOrder = m.DISPLAYORDER,
						Name = m.NAME
					}).FirstOrDefault();

				return state;
			}
		}

		public List<State> GetStatesByCountryCode(string countryCode)
		{
			using(Context = GetDataContext())
			{
				countryCode = countryCode.ToLower();

				var query = from s in Context.LST_STATE
				            join c in Context.LST_COUNTRY on s.COUNTRY_ID equals c.ID
				            where c.ISOCODE_2L.ToLower() == countryCode || c.ISOCODE_3L.ToLower() == countryCode
				            select new State
				                   	{
				                   		StateProvinceID = s.ID,
										CountryID = s.COUNTRY_ID,
										Abbreviation = s.ABBREVIATION,
										Name = s.NAME,
										DisplayOrder = s.DISPLAYORDER
				                   	};

				return query.ToList();
			}
		}

		public List<State> GetStateByCountry(int countryID)
		{
			using(Context = GetDataContext())
			{
				var state = Context.LST_STATE.Where(m => m.COUNTRY_ID == countryID).OrderBy(m => m.DISPLAYORDER)
					.Select(
						m =>
						new State
							{
								Abbreviation = m.ABBREVIATION,
								CountryID = m.COUNTRY_ID,
								DisplayOrder = m.DISPLAYORDER,
								Name = m.NAME,
								StateProvinceID = m.ID
							}).ToList();

				return state;
			}
		}
	}
}