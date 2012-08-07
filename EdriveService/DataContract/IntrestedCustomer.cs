using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EdriveService.DataContract
{
    [DataContract]
    public class IntrestedCustomer
    {
        [DataMember]
        public string firstname { get; set; }
        [DataMember]
        public string lastname { get; set; }
        [DataMember]
        public int productId { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string phoneno { get; set; }
        [DataMember]
        public int zipcode { get; set; }
        [DataMember]
        public string contactType { get; set; }
        [DataMember]
        public int intrestType { get; set; }
        [DataMember]
        public string additionalComments { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime createdOn { get; set; }
        [DataMember]
        public DateTime updateOn { get; set; }
        [DataMember]
        public bool finacing { get; set; }
        [DataMember]
        public bool Trade_in { get; set; }
        [DataMember]
        public bool price_Lock { get; set; }
        [DataMember]
        public int customerId { get; set; }

        [DataMember]
        public int InterestedCustomerID { get; set; }

    }
}