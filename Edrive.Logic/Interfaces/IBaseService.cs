using System.Data.Objects;

namespace Edrive.Logic.Interfaces
{
	public interface IBaseService<T> where T : ObjectContext
	{
		T Context { get; set; }

		T GetDataContext();
	}
}