namespace Edrive.Core.Model
{
	public class Picture
	{
		public int ID { get; set; }

		public byte[] Binary { get; set; }

		public string Extension { get; set; }

		public bool IsNew { get; set; }
	}
}