using Edrive.Core.Interfaces.Data;
using Edrive.Core.Interfaces.Service;
using Edrive.Core.Model;

namespace Edrive.Service
{
	public partial class ProductService : BaseService<Products>, IProductService
	{
		protected new IProductRepository Repository;
		
		public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository):base(unitOfWork)
		{
		    base.Repository = Repository = productRepository;
		}
	}
}