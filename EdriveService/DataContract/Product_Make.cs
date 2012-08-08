using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class Product_Make
    {
        [DataMember]
        public Int32 id { get; set; }
        [DataMember]
        public string make { get; set; }

    }
}
