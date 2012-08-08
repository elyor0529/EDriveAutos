using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class CustomerPay
    {
        public string URL { get; set; }

        public string business { get; set; }

        public string cmd { get; set; }

        public string item_name { get; set; }

        public string amount { get; set; }

        public string currency_code { get; set; }

        public string request_id { get; set; }

        public string ReturnUrl { get; set; }

        public string CancelPurchaseUrl { get; set; }
    }
}