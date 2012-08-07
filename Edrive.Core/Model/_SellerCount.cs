using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
     [DataContract]
    public class _SellerCount
    {
             [DataMember]
        public long Dealer { get; set; }
             [DataMember]
             public long Seller { get; set; }

    }
}