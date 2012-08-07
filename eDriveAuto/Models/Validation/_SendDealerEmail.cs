using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class _SendDealerEmail
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Phone { get; set; }
        [Required]
        public String City { get; set; }

        public String StateID { get; set; }

        public Int32 _prdID { get; set; }
        public String Subject { get; set; }

        [Required]
        public String Comments { get; set; }
    }
}