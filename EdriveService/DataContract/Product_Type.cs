using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
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
