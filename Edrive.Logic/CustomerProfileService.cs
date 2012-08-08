using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class CustomerProfileService : ICustomerProfileService
	{
		public _CustomerProfile GetProfile(int dealerID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var profile = entities.CustomerProfile.FirstOrDefault(m => m.CustomerID == dealerID);

				if(profile != null)
				{
					var customerProfile = new _CustomerProfile
					{
						CustomerID = profile.CustomerID ?? 0,
						ApplicationURL = profile.ApplicationURL,
						Description = profile.Description,
						Logo = profile.Logo,
						PageImage = profile.PageImage,
						ServiceURL = profile.ServiceURL,
						WarrantyURL = profile.WarrantyURL
					};

					return customerProfile;
				}

				return new _CustomerProfile();
			}
		}
	}
}