using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class _StateProvince
    {
        [DataMember]

        public int StateProvinceID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]

        public String Name { get; set; }


    }
}