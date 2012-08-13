using Edrive.Core.Enums;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using System.Linq;

namespace Edrive.Logic
{
	public class BuyerService : BaseService, IBuyerService
	{
		public Buyer GetByID(int id)
		{
			using(Context = GetDataContext())
			{
				var query = (from c in Context.BUYERs
				             where c.ID == id
				             select c).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public Buyer GetByUsername(string username)
		{
			using(Context = GetDataContext())
			{
				var query = (from c in Context.BUYERs 
							where c.EMAIL.ToLower() == username.ToLower() 
							select c).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public bool SaveBuyer(Buyer item)
		{
			using(Context = GetDataContext())
			{
				var buyer = (from c in Context.BUYERs where c.ID == item.ID select c).FirstOrDefault();
				
				if(buyer != null)
				{
					buyer.NAME_FIRST = item.FirstName;
					buyer.NAME_LAST = item.LastName;
					buyer.ADDRESS = item.Address;
					buyer.DATE_REGISTERED = item.RegistrationDate;
					buyer.DATE_EXPIRED = item.ExpirationDate;
					buyer.IPADDRESS = item.IPAddress;
					buyer.CITY = item.City;
					buyer.ISACTIVE = item.IsActive;
					buyer.ISDELETED = item.IsDeleted;
					buyer.ISNEWSLETTER = item.IsNewsLetter;
					buyer.ISTRIAL = item.IsTrial;
					buyer.TYPE_ID = item.TypeID;
					buyer.PASSWORD = item.Password;
					buyer.PHONE = item.Phone;
					buyer.ZIP = item.Zip;

					Context.SaveChanges();
				}
				else
				{
					var isExists = (Context.BUYERs.Where(c => c.EMAIL.ToLower() == item.Email.ToLower())).Any();

					if(isExists)
						return false;

					var newBuyer = new BUYER
						{
							EMAIL = item.Email,
							NAME_FIRST = item.FirstName,
							NAME_LAST = item.LastName,
							ADDRESS = item.Address,
							DATE_REGISTERED = item.RegistrationDate,
							DATE_EXPIRED = item.ExpirationDate,
							IPADDRESS = item.IPAddress,
							CITY = item.City,
							ISACTIVE = item.IsActive,
							ISDELETED = item.IsDeleted,
							ISNEWSLETTER = item.IsNewsLetter,
							ISTRIAL = item.IsTrial,
							TYPE_ID = item.TypeID,
							PASSWORD = item.Password,
							PHONE = item.Phone,
							ZIP = item.Zip
						};

					Context.BUYERs.AddObject(newBuyer);
					Context.SaveChanges();
				}

				return true;
			}
		}

		public Buyer Authenticate(string username, string password, UserType role)
		{
			using(Context = GetDataContext())
			{
				int userType = (int) role;

				var buyer = (from c in Context.BUYERs
				                where
					                c.EMAIL.ToLower() == username.ToLower() && c.PASSWORD == password && c.TYPE_ID == userType &&
					                c.ISDELETED == false
				                select c).Select(ConvertType).FirstOrDefault();

				return buyer;
			}
		}

		public bool ChangePassword(int id, string newPassword)
		{
			using(Context = GetDataContext())
			{
				bool result = false;
				var query = Context.BUYERs.FirstOrDefault(c => c.ID == id);

				if(query != null)
				{
					query.PASSWORD = newPassword;
					Context.SaveChanges();
					result = true;
				}

				return result;
			}
		}

		#region Private Methods
		
		private Buyer ConvertType(BUYER item)
		{
			var buyer = new Buyer
				{
					ID = item.ID,
					Zip = item.ZIP,
					FirstName = item.NAME_FIRST,
					LastName = item.NAME_LAST,
					City = item.CITY,
					IsNewsLetter = item.ISNEWSLETTER,
					Phone = item.PHONE,
					State = item.STATE,
					Address = item.ADDRESS,
					Email = item.EMAIL,
					ExpirationDate = item.DATE_EXPIRED,
					IPAddress = item.IPADDRESS,
					IsActive = item.ISACTIVE,
					IsDeleted = item.ISDELETED,
					IsTrial = item.ISTRIAL,
					Password = item.PASSWORD,
					RegistrationDate = item.DATE_REGISTERED,
					TypeID = item.TYPE_ID
				};

			return buyer;
		}

		#endregion
	}
}