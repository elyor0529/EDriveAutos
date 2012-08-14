using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface ITestimonialService
	{
		List<Testimonial> GetAll(int pageSize, int pageNumber, out int count);

		Testimonial GetByID(int id);

		int Save(Testimonial item);

		bool Delete(int id);
	}
}