using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class Products : PersistentEntity
    {
    	private string _pics = String.Empty;
        DateTime _CreatedOn, _UpdatedOn;
        
		public Products()
        { _UpdatedOn=_CreatedOn = DateTime.UtcNow; }
         [DataMember]
        public string Reserved { get; set; }
        [DataMember]

        public int productId { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public bool published { get; set; }
        [DataMember]
        public bool deleted { get; set; }
        [DataMember]
        public DateTime createdOn { get { return _CreatedOn; } set { _CreatedOn = value; } }
        [DataMember]
        public DateTime updatedOn { get { return _UpdatedOn; } set { _UpdatedOn = value; } }
        [DataMember]
        public String vin { get; set; }
        [DataMember]
        public String OwnerDetail { get; set; }
        [DataMember]
        public int customerId { get; set; }
        [DataMember]
        public string vehicleName { get; set; }

        
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string stock { get; set; }
        [DataMember]
        public string ModelName{ get; set; }
        [DataMember]
        public string MakeName { get; set; }
        
        [DataMember]

        public int Year { get; set; }
        [DataMember]
        public int model { get; set; }
        [DataMember]
        public string trim { get; set; }
        [DataMember]
        public string free_Text { get; set; }
        [DataMember]
        public string body { get; set; }

        [DataMember]
        public Int32 bodyID { get; set; }
        [DataMember]
        public Int32 mileage { get; set; }
        [DataMember]
        public decimal price_Current { get; set; }
        [DataMember]
		public decimal price_WholeSale
		{
			get { return (decimal)WholeSalePrice.GetValueOrDefault(0.00); }
			set { WholeSalePrice = (double?)value; }
		}

		[DataMember]
		public double? WholeSalePrice { get; set; }

    	[DataMember]
		public decimal price_Cost
		{
			get { return (decimal) PriceCost.GetValueOrDefault(0.00); }
			set { PriceCost = (double?)value; }
		}

		public double? PriceCost { get; set; }

    	[DataMember]
        public string condition { get; set; }
        [DataMember]
        public string exterior { get; set; }
        [DataMember]
        public string interior { get; set; }
        [DataMember]
        public int doors { get; set; }
        [DataMember]
        public string engine { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Title { get; set; }
        
        [DataMember]
        public string transmission { get; set; }
        [DataMember]
        public string fuel_Type { get; set; }
        [DataMember]
        public string drive_Type { get; set; }
        [DataMember]
        public String options { get; set; }
        [DataMember]
        public bool warranty { get; set; }
        [DataMember]
        public String descriptiont { get; set; }
    	[DataMember]
    	public String pics
    	{
    		get { return _pics ?? String.Empty; }
			set { _pics = value; }
    	}
        [DataMember]
        public DateTime date_in_Stock { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public bool isNew { get; set; }
        [DataMember]
        public bool isfeature { get; set; }
        [DataMember]
        public decimal savingAmount { get; set; }
        [DataMember]
        public int stateID { get; set; }
        [DataMember]
        public decimal qualifyPrice { get; set; }
        [DataMember]
        public decimal averageRetailPrice { get; set; }
        [DataMember]
        public decimal averageTradeinPrice { get; set; }
        [DataMember]
        public int city_Fuel { get; set; }
        [DataMember]
        public int highWay_Fuel { get; set; }
        [DataMember]
        public int zip { get; set; }
        [DataMember]
        public bool ShowOnDealerProfile { get; set; }

[DataMember]
        public Customer Customer{ get; set; }




         [DataMember]
        public string VehicleType { get; set; }
    }

    public class TempProducts
    {
        DateTime _CreatedOn, _UpdatedOn;
        public TempProducts()
        { _UpdatedOn = _CreatedOn = DateTime.UtcNow; }
        [DataMember]
        public string Reserved { get; set; }
        [DataMember]

        public int productId { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public bool published { get; set; }
        [DataMember]
        public bool deleted { get; set; }
        [DataMember]
        public DateTime createdOn { get { return _CreatedOn; } set { _CreatedOn = value; } }
        [DataMember]
        public DateTime updatedOn { get { return _UpdatedOn; } set { _UpdatedOn = value; } }
        [DataMember]
        public String vin { get; set; }
        [DataMember]
        public String OwnerDetail { get; set; }
        [DataMember]
        public int customerId { get; set; }
        [DataMember]
        public string vehicleName { get; set; }


        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string stock { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MakeName { get; set; }

        [DataMember]

        public int Year { get; set; }
        [DataMember]
        public int model { get; set; }
        [DataMember]
        public string trim { get; set; }
        [DataMember]
        public string free_Text { get; set; }
        [DataMember]
        public string body { get; set; }

        [DataMember]
        public Int32 bodyID { get; set; }
        [DataMember]
        public Int32 mileage { get; set; }
        [DataMember]
        public decimal price_Current { get; set; }
        [DataMember]
        public decimal price_WholeSale { get; set; }
        [DataMember]
        public decimal price_Cost { get; set; }
        [DataMember]
        public string condition { get; set; }
        [DataMember]
        public string exterior { get; set; }
        [DataMember]
        public string interior { get; set; }
        [DataMember]
        public int doors { get; set; }
        [DataMember]
        public string engine { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string transmission { get; set; }
        [DataMember]
        public string fuel_Type { get; set; }
        [DataMember]
        public string drive_Type { get; set; }
        [DataMember]
        public String options { get; set; }
        [DataMember]
        public bool warranty { get; set; }
        [DataMember]
        public String descriptiont { get; set; }
        [DataMember]
        public String pics { get; set; }
        [DataMember]
        public DateTime date_in_Stock { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public bool isNew { get; set; }
        [DataMember]
        public bool isfeature { get; set; }
        [DataMember]
        public decimal savingAmount { get; set; }
        [DataMember]
        public int stateID { get; set; }
        [DataMember]
        public decimal qualifyPrice { get; set; }
        [DataMember]
        public decimal averageRetailPrice { get; set; }
        [DataMember]
        public decimal averageTradeinPrice { get; set; }
        [DataMember]
        public int city_Fuel { get; set; }
        [DataMember]
        public int highWay_Fuel { get; set; }
        [DataMember]
        public int zip { get; set; }
        [DataMember]
        public bool ShowOnDealerProfile { get; set; }

        [DataMember]
        public Customer Customer { get; set; }




        [DataMember]
        public string VehicleType { get; set; }
    }
}
