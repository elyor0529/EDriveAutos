using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
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
