using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class Nop_CustomerAttribute
    {
        [DataMember]
        public int customerAttributeId { get; set; }
        [DataMember]
        public int customerId { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string value { get; set; }

    }
}
