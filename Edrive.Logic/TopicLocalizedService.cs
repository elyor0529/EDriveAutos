using Edrive.Core.Model;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class TopicLocalizedService : BaseService, ITopicLocalizedService
	{
		public bool Save(TopicLocalized item)
		{
			using(Context = GetDataContext())
			{
				return false;
			}
		}
	}
}