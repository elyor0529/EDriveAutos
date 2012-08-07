namespace EdriveService
{
	public class Helper
	{
		public static string GetZip(int? zip)
		{
			string result = string.Empty;

			if(zip.HasValue && zip > 0)
			{
				result = zip.ToString().PadLeft(5, '0');
			}

			return result;
		}
	}
}