using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class Product_Model
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string modelName { get; set; }
        [DataMember]
        public int makeID { get; set; }
    }
}
