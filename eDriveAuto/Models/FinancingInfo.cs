using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
     
    public class FinancingInfo
    {
        //personal information
        [Required]
        public string FirstName{ get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string StreedAddress { get; set; }
        [Required]
        public Int32? ZipCode { get; set; }
        [Required]
        public string City { get; set; }

         [Required]
        public string State { get; set; }
         [Required]
         [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Wrong Email")]
         public string Email { get; set; }
       // public string HomePhone { get; set; }
         public Int32? MobilePhone1 { get; set; }
         public Int32? MobilePhone2 { get; set; }
         public Int32? MobilePhone3 { get; set; }
         [Required]
        public string ResidenceType { get; set; }
         [Required]
        public Int32 HowLongAtAddressYear { get; set; }
         [Required]
         public Int32 HowLongAtAddressMonth { get; set; }
         [Required]
        public string MonthlyRent { get; set; }
       // public string ResidenceType { get; set; }

        //EmploymentInformation
          [Required]
        public string EmpolyerName{ get; set; }
          [Required]
        public string JobTitle { get; set; }
          public Int32? WorkPhone1 { get; set; }
          public Int32? WorkPhone2 { get; set; }
          public Int32? WorkPhone3 { get; set; }
         [Required]
        public Int32 WorkedForYear { get; set; }
         [Required]
         public Int32 WorkedForMonth { get; set; }

        //Income information
        [Required]
        public Int32 MonthlyIncome { get; set; }
        [Required]
        public Int32 DownPayment { get; set; }
        public Boolean IsBankrupt { get; set; }
        [Required]
        public String CreditScore { get; set; }
        [Required]
        public Boolean IsCoSigner { get; set; }


        public Int32? HomePhone1 { get; set; }
        public Int32? HomePhone2 { get; set; }
        public Int32? HomePhone3 { get; set; }


        public string HomePhone { 
            get{
                return HomePhone1+"-"+HomePhone2+"-"+HomePhone3;
            }  
        }

        public string MobilePhone
        {
            get
            {
                return MobilePhone1 + "-" + MobilePhone2 + "-" + MobilePhone3;
            }
           
        }

        public string WorkPhone
        {
            get
            {
                return WorkPhone1 + "-" + WorkPhone2 + "-" + WorkPhone3;
            }
            
        }
        public DateTime CreatedOn
        {
            get ;
            set;
        }
    }
}
