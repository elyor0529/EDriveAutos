using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class InterestedCustomerService : IInterestedCustomerService
	{
		public bool IntrestedCustomer(IntrestedCustomer customer, int productID)
		{
			try
			{
				using(EDriveEntities entities = new EDriveEntities())
				{
					var objCustomer = new ED_InterestedCustomer
					                  	{
					                  		ProductId = customer.productId,
					                  		FirstName = customer.firstname,
					                  		LastName = customer.lastname,
					                  		CustomerId = customer.customerId,
					                  		Email = customer.email,
					                  		PhoneNumber = customer.phoneno,
					                  		ContactType = entities.ContactType.First(m => m.ContactType1 == customer.contactType).id,
					                  		AdditionalComment = customer.additionalComments,
					                  		IsActive = true,
					                  		Financing = customer.finacing,
					                  		Trade_in = customer.Trade_in,
					                  		Price_Lock = customer.price_Lock,
					                  		CreatedOn = customer.createdOn,
					                  		UpdatedOn = customer.updateOn,
					                  		InterestType = customer.intrestType
					                  	};
					
					entities.ED_InterestedCustomer.AddObject(objCustomer);
					entities.SaveChanges();
					
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
			using(EDriveEntities entities = new EDriveEntities())
			{
				var lstInterested = entities.ED_InterestedCustomer.Where(m => m.IsActive == true && m.InterestType == interestType).ToList();
				var lstInterestedCustomer = new List<IntrestedCustomer>();

				foreach (var item in lstInterested)
				{
					var it = new IntrestedCustomer
							{
								additionalComments = item.AdditionalComment,
								contactType = item.ContactType == null ? "" : item.ContactType.Value.ToString(),
								createdOn = item.CreatedOn ?? DateTime.Now,
								customerId = item.CustomerId ?? 0,
								email = item.Email,
								finacing = item.Financing ?? false,
								firstname = item.FirstName,
								intrestType = item.InterestType ?? 0,
								IsActive = true,
								lastname = item.LastName,
								phoneno = item.PhoneNumber,
								price_Lock = item.Price_Lock ?? false,
								productId = item.ProductId ?? 0,
								Trade_in = item.Trade_in ?? false,
								updateOn = item.UpdatedOn ?? DateTime.Now,
								InterestedCustomerID = item.InterestedCustomerID
							};

					int zipcode;
					if (int.TryParse(item.ZipCode, out zipcode))
					{
						it.zipcode = zipcode;
					}

					lstInterestedCustomer.Add(it);
				}

				return lstInterestedCustomer;
			}
		}
	}
}