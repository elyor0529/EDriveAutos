using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class AdvancedSearchAttributes
    {
        [DataMember]
        public Int32 _body { get; set; }
        [DataMember]
        public string _engine { get; set; }
        [DataMember]
        public Int32 _mileageFrom { get; set; }
        [DataMember]
        public Int32 _mileageTo { get; set; }
        [DataMember]
        public string _transmission { get; set; }
        [DataMember]
        public Int32 _zip { get; set; }
        [DataMember]
        public string _vin { get; set; }
        [DataMember]
        public string _radius { get; set; }
        [DataMember]
        public Int32 _make{ get; set; }
        
        [DataMember]
        public Int32 _model { get; set; }
        [DataMember]
        public int _minYaer { get; set; }
        [DataMember]
        public int _maxYear { get; set; }
        [DataMember]
        public decimal _minPrice { get; set; }
        [DataMember]
        public decimal _maxPrice { get; set; }
        [DataMember]
        public String _driveType { get; set; }
        [DataMember]
        public Int32  _Type { get; set; }

    }
}