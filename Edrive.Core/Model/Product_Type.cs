using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
  public class Product_Type
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string type { get; set; }

    }
}
