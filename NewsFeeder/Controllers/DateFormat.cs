using System;

namespace NewsFeeder.Controllers
{
	public class DateFormat
	{
		public static string GetTime(string date)
		{
			return DateTime.Parse(date).ToString();
		}
	}
}
