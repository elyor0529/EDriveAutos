using System.Linq;
using Edrive.Core.Model;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class CustomerProfileService : BaseService, ICustomerProfileService
	{
		public _CustomerProfile GetProfile(int dealerID)
		{
			using(Context = GetDataContext())
			{
				var profile = Context.DEALER_PROFILE.FirstOrDefault(m => m.DEALER_ID == dealerID);

				if(profile != null)
				{
					var customerProfile = new _CustomerProfile
					{
						CustomerID = profile.DEALER_ID ?? 0,
						ApplicationURL = profile.APPLICATION_URL,
						Description = profile.DESCRIPTION,
						Logo = profile.LOGO,
						PageImage = profile.PAGE_IMAGE,
						ServiceURL = profile.SERVICE_URL,
						WarrantyURL = profile.WARRANTY_URL
					};

					return customerProfile;
				}

				return new _CustomerProfile();
			}
		}
	}
}