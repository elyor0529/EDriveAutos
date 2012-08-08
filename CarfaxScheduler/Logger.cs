using System;
using System.IO;

namespace CarfaxScheduler
{
	/// <summary>
	/// Helper class for logging
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// Writes log to a file provided in the app.config file
		/// </summary>
		/// <param name="message">Text to write to the log file</param>
		/// <param name="stringFormat">Comma separated list of object to put into the string format</param>
		public static void WriteLog(string message, params object[] stringFormat)
		{
			try
			{
				File.AppendAllText(Settings.Default.LogFilePath, "-----------------------------------------------------------------------------" + Environment.NewLine);
				File.AppendAllText(Settings.Default.LogFilePath, DateTime.Now + " - " + String.Format(message, stringFormat) + Environment.NewLine);
				File.AppendAllText(Settings.Default.LogFilePath, "-----------------------------------------------------------------------------" + Environment.NewLine);
			}
			catch
			{
				//should never happen.
			}
		}
	}
}