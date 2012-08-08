using System.Collections.Generic;
using Edrive.Core.Model;
using Edrive.Data;
using System.Linq;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class StateProvinceService : IStateProvinceService
	{
		public List<_StateProvince> GetStatesByCountryCode(string countryCode)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				countryCode = countryCode.ToLower();

				var query = from s in entities.StateProvince
				            join c in entities.Country on s.CountryID equals c.CountryID
				            where c.TwoLetterISOCode.ToLower() == countryCode || c.ThreeLetterISOCode.ToLower() == countryCode
				            select new _StateProvince
				                   	{
				                   		StateProvinceID = s.StateProvinceID,
										CountryID = s.CountryID,
										Abbreviation = s.Abbreviation,
										Name = s.Name
				                   	};

				return query.ToList();
			}
		}
	}
}