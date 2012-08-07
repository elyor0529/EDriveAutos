using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class ProductOptions
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string OptionName { get; set; }
        
    }
}