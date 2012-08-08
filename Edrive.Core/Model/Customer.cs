using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
	[DataContract]
	public class Customer
	{
		[DataMember]
		public Int32 customerID { get; set; }

		[DataMember]
		public String customerGUID { get; set; }
		
		[DataMember]
		public String Name { get; set; }

		[DataMember]
		public String CompanyName { get; set; }

		[DataMember]
		public Int32 customerType { get; set; }

		[DataMember]
		public string email { get; set; }

		[DataMember]
		public bool active { get; set; }

		[DataMember]
		public DateTime registrationDate { get; set; }

		[DataMember]
		public String password { get; set; }

		[DataMember]
		public string iPAddress { get; set; }

		[DataMember]
		public DateTime? ExpiryDate { get; set; }

		[DataMember]
		public bool isTrial { get; set; }

		[DataMember]
		public String Gender { get; set; }

		[DataMember]
		public String FirstName { get; set; }

		[DataMember]
		public Int32 Zip { get; set; }
		
		[DataMember]
		public String LastName { get; set; }

		[DataMember]
		public DateTime? DateofBirth { get; set; }
		
		[DataMember]
		public String StreetAddress1 { get; set; }
		
		[DataMember]
		public String StreetAddress2 { get; set; }

		[DataMember]
		public String City { get; set; }

		[DataMember]
		public Int32 StateID { get; set; }
		
		[DataMember]
		public String StateName { get; set; }
		
		[DataMember]
		public String Phone { get; set; }

		[DataMember]
		public String Fax { get; set; }

		[DataMember]
		public Boolean Newsletter { get; set; }
		
		[DataMember]
		public Boolean IsFeatured { get; set; }

		[DataMember]
		public _CustomerProfile Profile { get; set; }
	}
}
