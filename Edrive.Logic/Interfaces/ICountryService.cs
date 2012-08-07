using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface ICountryService
	{
		List<Country> GetAll();
	}
}