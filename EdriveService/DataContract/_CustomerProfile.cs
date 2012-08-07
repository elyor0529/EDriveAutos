using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class _CustomerProfile
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public String Logo { get; set; }

        [DataMember]
        public String ApplicationURL { get; set; }
        [DataMember]
        public String WarrantyURL { get; set; }
        [DataMember]
        public String ServiceURL { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String PageImage { get; set; }




    }
}