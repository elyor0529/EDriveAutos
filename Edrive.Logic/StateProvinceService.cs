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
				var state = Context.StateProvince.Where(m => m.StateProvinceID == stateID)
					.Select(m => new State
					{
						StateProvinceID = m.StateProvinceID,
						Abbreviation = m.Abbreviation,
						CountryID = m.CountryID,
						DisplayOrder = m.DisplayOrder,
						Name = m.Name
					}).FirstOrDefault();

				return state;
			}
		}

		public List<State> GetStatesByCountryCode(string countryCode)
		{
			using(Context = GetDataContext())
			{
				countryCode = countryCode.ToLower();

				var query = from s in Context.StateProvince
				            join c in Context.Country on s.CountryID equals c.CountryID
				            where c.TwoLetterISOCode.ToLower() == countryCode || c.ThreeLetterISOCode.ToLower() == countryCode
				            select new State
				                   	{
				                   		StateProvinceID = s.StateProvinceID,
										CountryID = s.CountryID,
										Abbreviation = s.Abbreviation,
										Name = s.Name,
										DisplayOrder = s.DisplayOrder
				                   	};

				return query.ToList();
			}
		}

		public List<State> GetStateByCountry(int countryID)
		{
			using(Context = GetDataContext())
			{
				var state = Context.StateProvince.Where(m => m.CountryID == countryID).OrderBy(m => m.DisplayOrder)
					.Select(
						m =>
						new State
							{
								Abbreviation = m.Abbreviation,
								CountryID = m.CountryID,
								DisplayOrder = m.DisplayOrder,
								Name = m.Name,
								StateProvinceID = m.StateProvinceID
							}).ToList();

				return state;
			}
		}
	}
}