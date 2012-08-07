using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class Product
    {
        public int productId { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
        public bool deleted { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
        public String vin { get; set; }
        public String OwnerDetail { get; set; }
        public int customerId { get; set; }
        public string vehicleName { get; set; }
        public int type { get; set; }
        public string stock { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public int model { get; set; }
        public string trim { get; set; }
        public string free_Text { get; set; }
        public string body { get; set; }
        public Int32 mileage { get; set; }
        public decimal price_Current { get; set; }
        public bool condition { get; set; }
        public string exterior { get; set; }
        public string interior { get; set; }
        public int doors { get; set; }
        public string engine { get; set; }
        public string Make { get; set; }
        public string transmission { get; set; }
        public string fuel_Type { get; set; }
        public string drive_Type { get; set; }
        public String options { get; set; }
        public bool warranty { get; set; }
        public String descriptiont { get; set; }
        public String pics { get; set; }
        public DateTime date_in_Stock { get; set; }
        public string fileName { get; set; }
        public bool isNew { get; set; }
        public bool isfeature { get; set; }
        public decimal savingAmount { get; set; }
        public int stateID { get; set; }
        public decimal qualifyPrice { get; set; }
        public decimal averageRetailPrice { get; set; }
        public decimal averageTradeinPrice { get; set; }
        public int city_Fuel { get; set; }
        public int highWay_Fuel { get; set; }
        public int zip { get; set; }
       
    }
}