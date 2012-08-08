using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class Nop_StateProvince
    {
        [DataMember]
        public Int32 stateProvinceID { get; set; }
        [DataMember]
        public Int32 countryID { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string abbrevation { get; set; }
        [DataMember]
        public int displayOrder { get; set; }
    }
}
