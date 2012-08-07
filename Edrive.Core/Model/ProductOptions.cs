using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
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