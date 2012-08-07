using System;
using System.Collections.Generic;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Customer = Edrive.Core.Model.Customer;
using System.Linq;

namespace Edrive.Logic
{
	public class DealerService : IDealerService
	{
		public Customer GetByID(int customerID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var dealer = entities.Customer.FirstOrDefault(m => m.CustomerID == customerID);

				if (dealer == null)
					return null;

				if (String.IsNullOrEmpty(dealer.FirstName) && dealer.Name != null)
				{
					if (dealer.Name.IndexOf(' ') > 0)
					{
						dealer.FirstName = dealer.Name.Substring(0, dealer.Name.IndexOf(' ')).Trim();
						dealer.LastName = dealer.Name.Substring(dealer.Name.IndexOf(' ')).Trim();
					}
					else
					{
						dealer.FirstName = dealer.Name;
						dealer.LastName = "";
					}
				}

				var result = new Customer
				             	{
				             		customerID = dealer.CustomerID,
				             		email = dealer.Email ?? "",
				             		Name = dealer.Name ?? "",
				             		CompanyName = dealer.Company ?? "",
				             		Zip = dealer.ZipPostalCode ?? 0,
				             		FirstName = dealer.FirstName ?? "",
				             		LastName = dealer.LastName ?? "",
				             		StateName = dealer.StateProvince == null ? "" : dealer.StateProvince.Name,
				             		StreetAddress1 = dealer.StreetAddress ?? "",
				             		StreetAddress2 = dealer.StreetAddress2 ?? "",
				             		DateofBirth = dealer.DateOfBirth,
				             		City = dealer.City ?? "",
				             		StateID = dealer.Stateid ?? -1,
				             		Phone = dealer.Phone ?? "",
				             		password = dealer.Password ?? "",
				             		Fax = dealer.Fax ?? "",
				             		registrationDate = dealer.RegistrationDate,
				             		Newsletter = dealer.newsletter ?? false
				             	};

				result.Name = string.Format("{0} {1}", result.FirstName ?? String.Empty, result.LastName ?? String.Empty);
				
				return result;
			}
		}

		public Customer GetDealerByProductID(int productID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var customerID = entities.Product.Where(m => m.ProductId == productID).Select(m => m.CustomerID).SingleOrDefault();
				
				if (customerID == null)
					return null;

				var dealer = entities.Customer
					.Where(m => m.CustomerID == customerID)
					.Select(m => new Customer
					        	{
					        		customerID = m.CustomerID,
					        		email = m.Email,
					        		Name = m.Name,
					        		FirstName = m.FirstName,
					        		LastName = m.LastName,
					        		CompanyName = m.Company,
					        		Phone = m.Phone,
					        		City = m.City,
					        		StateID = m.Stateid ?? 0,
					        		StateName = m.StateProvince.Name,
					        		StreetAddress1 = m.StreetAddress,
					        		StreetAddress2 = m.StreetAddress2,
					        		Zip = m.ZipPostalCode ?? 0,
					        	}).FirstOrDefault();
				
				if(dealer != null)
					dealer.Name = string.Format("{0} {1}", dealer.FirstName ?? String.Empty, dealer.LastName ?? String.Empty);
				
				return dealer;
			}
		}

		public _CustomerProfile GetProfile(int dealerID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var profile = entities.CustomerProfile.FirstOrDefault(m => m.CustomerID == dealerID);
				
				if (profile != null)
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

		public Customer GetDealerByDealerEmail(string email)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var dealer = entities.Customer.Where(m => m.Email == email).Select(m => m).FirstOrDefault();
				
				if (dealer == null)
					return null;

				var customer = new Customer
				            	{
				            		active = dealer.Active,
				            		customerGUID = dealer.CustomerGUID.ToString(),
				            		customerType = dealer.CustomerType,
				            		customerID = dealer.CustomerID,
				            		DateofBirth = dealer.DateOfBirth,
				            		ExpiryDate = dealer.ExpiryDate,
				            		Gender = dealer.Gender,
				            		iPAddress = dealer.IPAddress,
				            		isTrial = dealer.IsTrial ?? false,
				            		Newsletter = dealer.newsletter ?? false,
				            		registrationDate = dealer.RegistrationDate,
				            		email = dealer.Email,
				            		Name = dealer.Name,
				            		CompanyName = dealer.Company,
				            		Zip = dealer.ZipPostalCode ?? 0,
				            		FirstName = dealer.FirstName,
				            		LastName = dealer.LastName,
				            		StreetAddress1 = dealer.StreetAddress,
				            		StreetAddress2 = dealer.StreetAddress2,
				            		City = dealer.City,
				            		StateID = dealer.Stateid ?? 0,
				            		Phone = dealer.Phone,
				            		password = dealer.Password,
				            		Fax = dealer.Fax,
				            		StateName = (dealer.StateProvince == null ? "" : dealer.StateProvince.Name),
				            	};
				
				customer.Name = string.Format("{0} {1}", customer.FirstName ?? String.Empty, customer.LastName ?? String.Empty);

				return customer;
			}
		}

		public int GetTotalDealersCount()
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				int count = entities.Customer.Count(m => m.Deleted == false && m.Customer_Type.Role == "Dealer");

				return count;
			}
		}

		public List<Customer> GetDealersByZip(string zipcode)
		{
			List<Customer> result = new List<Customer>();
			int zip;

			if(!int.TryParse(zipcode, out zip))
				return result;

			using(EDriveEntities entities = new EDriveEntities())
			{
				result = entities.Customer
					.Where(c => c.ZipPostalCode == zip && c.Deleted == false && c.Active == true)
					.Select(m =>
					        new Customer
					        	{
					        		customerID = m.CustomerID,
					        		email = m.Email,
					        		Name = m.Name,
					        		FirstName = m.FirstName,
					        		LastName = m.LastName,
					        		CompanyName = m.Company,
					        		Phone = m.Phone,
					        		City = m.City,
					        		StateID = m.Stateid ?? 0,
					        		StateName = m.StateProvince.Name,
					        		StreetAddress1 = m.StreetAddress,
					        		StreetAddress2 = m.StreetAddress2,
					        		Zip = m.ZipPostalCode ?? 0,
					        	}).ToList();

				return result;
			}
		}
	}
}