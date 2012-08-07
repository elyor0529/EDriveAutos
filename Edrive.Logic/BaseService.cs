using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class BaseService : IBaseService<EdriveAutosEntities>
	{
		public EdriveAutosEntities Context { get; set; }

		public EdriveAutosEntities GetDataContext()
		{
			return new EdriveAutosEntities();
		}
	}
}