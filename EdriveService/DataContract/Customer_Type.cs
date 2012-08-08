using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{

    [DataContract]
    public class Customer_Type
    {
        [DataMember]
        public Int32 id { get; set; }
        [DataMember]
        public string role { get; set; }
    }
}
