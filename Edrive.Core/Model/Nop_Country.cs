using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class Nop_Country
    {
        [DataMember]
        public Int32 countryID { get; set; }
        [DataMember]
        public string name
        {
            get;
            set;
        }
        [DataMember]
        public bool allowRegistration { get; set; }
        [DataMember]
        public bool allowBilling { get; set; }
        [DataMember]
        public bool allowShipping { get; set; }
        [DataMember]
        public string twoLetterISOCode { get; set; }
        [DataMember]
        public string threeLetterISOCode { get; set; }
        [DataMember]
        public Int32 numberISOCode { get; set; }
        [DataMember]
        public int published { get; set; }
        [DataMember]
        public Int32 displayOrder { get; set; }

    }
}
