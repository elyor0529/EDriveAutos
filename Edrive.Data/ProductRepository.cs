using Edrive.Core.Interfaces.Data;
using Edrive.Core.Model;

namespace Edrive.Data
{
	public partial class ProductRepository : BaseRepository<Products>, IProductRepository
	{
		public ProductRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}