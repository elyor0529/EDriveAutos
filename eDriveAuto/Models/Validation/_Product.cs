using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Edrivie_Service_Ref
{
    [MetadataType(typeof(_Product))]
    public partial class Products
    {
    }
    
     class _Product 
    {
               
              public int productId  { get; set; }
            
              public string name { get; set; }
             
              public bool published { get; set; }
             
              public bool deleted { get; set; }

              [Required(ErrorMessage = "*")]
              public decimal price_WholeSale { get; set; }
             
              public decimal price_Cost { get; set; }
              public DateTime createdOn { get; set; }
              [DataType(DataType.Date)]
         
              public DateTime updatedOn { get; set; }
             
          [Required(ErrorMessage="*")]
          [Remote("IsProductExist", "ManageVehicle", AdditionalFields = "productId",ErrorMessage= "This VIN already exists.")]
              public String vin { get; set; }
             
              public String OwnerDetail { get; set; }
             
              public int customerId { get; set; }
             
              public string vehicleName { get; set; }


              [Required(ErrorMessage="*")]
              public int type { get; set; }
               [Required(ErrorMessage="*")]
              public string stock { get; set; }

             [Required(ErrorMessage="*")]
              public string ModelName { get; set; }


            [Required(ErrorMessage="*")]
              public int Year { get; set; }
           
              public int model { get; set; }
             
              public string trim { get; set; }
             
              public string free_Text { get; set; }
               [Required(ErrorMessage="*")]
       
              public string body { get; set; }
               [Required(ErrorMessage="*")]
              public Int32 mileage { get; set; }
                [Required(ErrorMessage="*")]
              public decimal price_Current { get; set; }
             
              public bool condition { get; set; }
               [Required(ErrorMessage="*")]
              public string exterior { get; set; }
               [Required(ErrorMessage="*")] 
              public string interior { get; set; }
              [Required(ErrorMessage = "*")] 
              public int doors { get; set; }
             
              public string engine { get; set; }
              [Required(ErrorMessage="*")]
              public string Make { get; set; }

              [Required(ErrorMessage="*")]
              public string transmission { get; set; }
             
              public string fuel_Type { get; set; }
             
              public string drive_Type { get; set; }
             
              public String options { get; set; }
             
              public bool warranty { get; set; }
             
              public String descriptiont { get; set; }
             
              public String pics { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "*")]
              public DateTime date_in_Stock { get; set; }
             
              public string fileName { get; set; }
             
              public bool isNew { get; set; }
              public string Reserved { get; set; }
             
              public bool isfeature { get; set; }
             
              public decimal savingAmount { get; set; }
             
              public int stateID { get; set; }
             
              public decimal qualifyPrice { get; set; }
             
              public decimal averageRetailPrice { get; set; }
             
              public decimal averageTradeinPrice { get; set; }
             
              public int city_Fuel { get; set; }
             
              public int highWay_Fuel { get; set; }
             
              public int zip { get; set; }


              //public decimal price_Cost { get; set; }
              //public decimal price_WholeSale { get; set; }


          
    }
   
}