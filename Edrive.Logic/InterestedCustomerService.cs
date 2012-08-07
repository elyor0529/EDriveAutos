using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class InterestedCustomerService : BaseService, IInterestedCustomerService
	{
		public bool IntrestedCustomer(IntrestedCustomer customer, int productID)
		{
			try
			{
				using(Context = GetDataContext())
				{
					var buyerInterested = new BUYER_INTERESTED
					                  	{
					                  		VEHICLE_ID = customer.productId,
					                  		NAME_FIRST = customer.firstname,
					                  		NAME_LAST = customer.lastname,
					                  		BUYER_ID = customer.customerId,
					                  		EMAIL = customer.email,
					                  		PHONE = customer.phoneno,
					                  		COMMENTS = customer.additionalComments,
					                  		ISACTIVE = true,
					                  		ISFINANCE = customer.finacing,
					                  		ISTRADEIN = customer.Trade_in,
					                  		ISPRICELOCK = customer.price_Lock,
					                  		DATE_CREATED = customer.createdOn,
					                  		DATE_UPDATED = customer.updateOn,
					                  		INTEREST_TYPE = customer.intrestType,
											ZIPCODE = customer.zipcode.ToString().PadLeft(5)
					                  	};
					
					Context.BUYER_INTERESTED.AddObject(buyerInterested);
					Context.SaveChanges();
					
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public List<IntrestedCustomer> Get_InterestedCustomer(int interestType)
		{
			using(Context = GetDataContext())
			{
				var interestedCustomers =
					Context.BUYER_INTERESTED.Where(m => m.ISACTIVE == true && m.INTEREST_TYPE == interestType).Select(
						item => new IntrestedCustomer
						        	{
						        		additionalComments = item.COMMENTS,
						        		createdOn = item.DATE_CREATED ?? DateTime.Now,
						        		customerId = item.BUYER_ID,
						        		email = item.EMAIL,
						        		finacing = item.ISFINANCE ?? false,
						        		firstname = item.NAME_FIRST,
						        		intrestType = item.INTEREST_TYPE ?? 0,
						        		IsActive = true,
						        		lastname = item.NAME_LAST,
						        		phoneno = item.PHONE,
						        		price_Lock = item.ISPRICELOCK ?? false,
						        		productId = item.VEHICLE_ID ?? 0,
						        		Trade_in = item.ISTRADEIN ?? false,
						        		updateOn = item.DATE_UPDATED ?? DateTime.Now,
						        		InterestedCustomerID = item.ID,
										Zip = item.ZIPCODE
						        	}).ToList();

				return interestedCustomers;
			}
		}
	}
}