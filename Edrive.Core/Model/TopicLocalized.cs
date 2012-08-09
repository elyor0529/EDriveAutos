using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Edrive.Core.Model
{
	[DataContract]
	public class TopicLocalized
	{
		[DataMember]
		public int TopicLocalizedID { get; set; }

		[Required]
		[DisplayName("Topic")]
		[DataMember]
		public String TopicName { get; set; }

		[DataMember]
		public int TopicID { get; set; }

		[DataMember]
		public string Title { get; set; }

		[AllowHtml]
		[DataMember]
		public string Body { get; set; }

		[DisplayName("Created On")]
		[DataMember]
		public DateTime CreatedOn { get; set; }

		[DisplayName("Updated On")]
		[DataMember]
		public DateTime UpdatedOn { get; set; }

		[DisplayName("Meta Title")]
		[DataMember]
		public string MetaTitle { get; set; }

		[DisplayName("Meta keywords")]
		[DataMember]
		public string MetaKeywords { get; set; }

		[DisplayName("Meta description")]
		[DataMember]
		public string MetaDescription { get; set; }
	}
}