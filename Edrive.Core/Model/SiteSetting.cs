using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
	[DataContract]
	public class SiteSetting
	{
		[DataMember]
		public int ID { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Value { get; set; }
	}
}