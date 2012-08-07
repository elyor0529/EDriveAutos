using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{

    [DataContract]
    public class ED_Zipcodes
    {
        [DataMember]
        public double latitude { get; set; }
        [DataMember]
        public double longitude { get; set; }
        [DataMember]
        public string zip_code { get; set; }

    }
}