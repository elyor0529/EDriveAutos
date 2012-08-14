using System;
using System.IO;
using System.Web;
using Edrive.Core.Model;
using Edrive.Logic;

namespace Edrive.CommonHelpers
{
	public static class PictureManager
	{
		public static int SavePicture(HttpPostedFileBase picture)
		{
			int pictureID = 0;

			try
			{
				if(picture != null && picture.ContentLength > 0)
				{
					var length = picture.ContentLength;
					byte[] filsbyte = new byte[length];
					picture.InputStream.Read(filsbyte, 0, length);
					var ext = Path.GetExtension(picture.FileName);

					if(ext != null)
						ext = ext.Trim('.');
					else
						return 0;

					var image = new Picture
						{
							Binary = filsbyte,
							IsNew = true,
							Extension = string.Format("image/{0}", ext)
						};
					
					PictureService service = new PictureService();

					pictureID = service.Save(image);
					
					return pictureID;
				}
			}
			catch(Exception)
			{
				return pictureID;
			}

			return pictureID;
		}
	}
}