using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{

    /// <summary>
    /// This class is created for Dealre Manage Search Filter
    /// </summary>
    public class _DealerManageFilter
    {
        [DisplayName("Registration from")]
        public DateTime?  RegFrom{ get; set; }
        [DisplayName("Registration to")]
        public DateTime? RegTo { get; set; }
        [DisplayName("Last Name")]
        public String LastName { get; set; }
           [DisplayName("Email")]
        public String Email { get; set; }
              [DisplayName("LastName")]
        public String Name { get; set; }
        [DisplayName("Dealership Name")]
        public String CompanyName{ get; set; }
         [DisplayName("Dealer Name")]
        public String CompanyName2 { get; set; }

      


    }
}