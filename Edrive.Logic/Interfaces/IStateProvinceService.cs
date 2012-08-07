using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IStateProvinceService
	{
		State GetByID(int stateID);

		List<State> GetStatesByCountryCode(string countryCode);

		List<State> GetStateByCountry(int countryID);
	}
}