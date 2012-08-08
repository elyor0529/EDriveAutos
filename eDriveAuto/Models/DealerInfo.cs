using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class DealerInfo
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
        [MyRemote("IsCustomerExist", "Common", areaName: "", AdditionalFields = "CustomerID", ErrorMessage = "This Email already exists.")]
     
     //   [Remote("IsDealerExist", "ManageDealer", AdditionalFields = "CustomerID", ErrorMessage = "This Email already exists.")]
        public String  Email { get; set; }
        [Required]
        //[DataType(DataType.Password)]
        public String Password { get; set; }
        [Required]
        public String Gender { get; set; }

        
        [Required]
        [DisplayName("First Name")]
        public String FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public String LastName { get; set; }
        
        [DisplayName("Date of Birth")]
        public DateTime? DateofBirth { get; set; }
        [Required]
        public String Company { get; set; }
        [Required]
        [DisplayName("Street Address1")]
        public String  StreetAddress1 { get; set; }
        [DisplayName("Street Address2")]
        public String StreetAddress2 { get; set; }
        [Required]
        public Int32 Zip { get; set; }
        [Required]
        public String City { get; set; }

        [Display(Name = "State / province")]
        [Required]
        public Int32 StateID { get; set; }

       // public Int32 CountryID{ get; set; }
        
        [Required]
        public  String  Phone{ get; set; }
        
        public  String  Fax{ get; set; }
         [Required]
        public  Boolean  Newsletter{ get; set; }

        
         public int CustomerID { get; set; }
        
         public String Logo { get; set; }

        [RegularExpression(@"(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?"
            , ErrorMessage = "Invalid Application URL")]
         public String ApplicationURL { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]

        [RegularExpression(@"(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?",
            ErrorMessage = "Invalid Warranty URL")]
        public String WarrantyURL { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]

        [RegularExpression(@"(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?", ErrorMessage = "Invalid Service URL")]
        public String ServiceURL { get; set; }
        
         [AllowHtml]
         public String Description { get; set; }
        
         public String PageImage { get; set; }


         public DateTime RegisterationDate { get; set; }
       
    }
}