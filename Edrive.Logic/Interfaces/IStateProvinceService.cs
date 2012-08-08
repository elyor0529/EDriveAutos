using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IStateProvinceService
	{
		List<_StateProvince> GetStatesByCountryCode(string countryCode);
	}
}