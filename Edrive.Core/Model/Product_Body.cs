using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class Product_Body
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string body { get; set; }
    }
}
