using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
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
