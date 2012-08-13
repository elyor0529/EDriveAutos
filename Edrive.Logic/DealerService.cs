using System;
using System.Collections.Generic;
using Edrive.Core.Enums;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Customer = Edrive.Core.Model.Customer;
using System.Linq;

namespace Edrive.Logic
{
	public class DealerService : BaseService, IDealerService
	{
		public Customer GetByID(int dealerID)
		{
			using(Context = GetDataContext())
			{
				var dealer = (from c in Context.DEALERs
				              join p in Context.DEALER_SALESPERSON on c.ID equals p.DEALER_ID
				              where c.ID == dealerID
							  select p).Select(ConvertType).FirstOrDefault();

				if (dealer == null)
					return null;

				SetDealerName(dealer);
				
				return dealer;
			}
		}

		public Customer GetDealerByProductID(int productID)
		{
			using(Context = GetDataContext())
			{
				var customerID = Context.VEHICLEs.Where(m => m.ID == productID).Select(m => m.SALESPERSON_ID).SingleOrDefault();
				
				if (customerID == null)
					return null;

				var dealer = (from c in Context.DEALERs
				              join p in Context.DEALER_SALESPERSON on c.ID equals p.DEALER_ID
				              where p.ID == customerID
							  select p).Select(ConvertType).FirstOrDefault();

				SetDealerName(dealer);
				
				return dealer;
			}
		}
		
		public Customer GetDealerByDealerEmail(string email)
		{
			using(Context = GetDataContext())
			{
				email = email.ToLower().Trim();

				var dealer = (from c in Context.DEALERs
				              join p in Context.DEALER_SALESPERSON on c.ID equals p.DEALER_ID
				              where c.EMAIL.ToLower() == email
							  select p).Select(ConvertType).FirstOrDefault();

				if (dealer == null)
					return null;
				
				SetDealerName(dealer);

				return dealer;
			}
		}

		public int GetTotalDealersCount()
		{
			using(Context = GetDataContext())
			{
				int userType = (int)UserType.Dealer;
				int count = Context.DEALER_SALESPERSON.Count(m => m.ISDELETED == false && m.DEALERTYPE_ID == userType);

				return count;
			}
		}

		public List<Customer> GetDealersByZip(string zipcode)
		{
			List<Customer> result = new List<Customer>();
			int zip;

			if(!int.TryParse(zipcode, out zip))
				return result;

			using(Context = GetDataContext())
			{
				var dealers = (from c in Context.DEALERs
				             join p in Context.DEALER_SALESPERSON on c.ID equals p.DEALER_ID
				             where c.ZIP == zipcode && p.ISACTIVE && p.ISDELETED == false
							   select p).Select(ConvertType).ToList();

				return dealers;
			}
		}
		
		public bool ChangePassword(int dealerID, string newPassword)
		{
			using(Context = GetDataContext())
			{
				bool result = false;
				var query = Context.DEALER_SALESPERSON.FirstOrDefault(c => c.ID == dealerID);

				if(query != null)
				{
					query.PASSWORD = newPassword;
					Context.SaveChanges();
					result = true;
				}

				return result;
			}
		}

		#region private methods

		private void SetDealerName(Customer dealer)
		{
			string firstName = dealer.Name;
			string lastName = String.Empty;

			if(!String.IsNullOrWhiteSpace(dealer.Name))
			{
				var nameArr = dealer.Name.Split(' ');

				if(nameArr.Length > 1)
				{
					firstName = nameArr[0];
					lastName = nameArr[1];
				}
			}

			dealer.FirstName = firstName;
			dealer.LastName = lastName;
		}

		private Customer ConvertType(DEALER_SALESPERSON salesPerson)
		{
			var customer = new Customer
			               	{
			               		ZipCode = salesPerson.DEALER.ZIP,
			               		Name = salesPerson.NAME,
			               		City = salesPerson.DEALER.CITY,
			               		customerID = salesPerson.DEALER.ID,
			               		active = salesPerson.ISACTIVE,
			               		isTrial = salesPerson.ISTRIAL ?? true,
			               		email = salesPerson.DEALER.EMAIL,
			               		StateName = salesPerson.DEALER.STATE,
			               		StreetAddress1 = salesPerson.DEALER.ADDRESS,
			               		CompanyName = salesPerson.DEALER.NAME,
			               		Newsletter = salesPerson.NEWSLETTER ?? false,
			               		Phone = salesPerson.DEALER.PHONE,
			               		IsFeatured = salesPerson.ISFEATURED ?? false,
			               		registrationDate = salesPerson.DATE_REGISTRATION,
			               		iPAddress = salesPerson.IPADDRESS,
			               		password = salesPerson.PASSWORD,
			               		ExpiryDate = salesPerson.DATE_EXPIRES,
			               		Fax = salesPerson.FAX,
			               		customerType = salesPerson.DEALERTYPE_ID
			               	};

			return customer;
		}

		#endregion
	}
}