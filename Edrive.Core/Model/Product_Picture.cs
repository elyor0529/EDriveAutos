using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    [DataContract]
    public class Product_Picture
    {
        [DataMember]
        public int ProductPictureID { get; set; }
        [DataMember] public int ProductID { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public string PictureURL { get; set; }

    }
}