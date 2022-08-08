using System;

namespace NewsFeeder.Utilities
{
	public class DateUtilities
	{
		public static string FormateDate(string date)
		{
			return DateTime.Parse(date).ToString();
		}
	}
}
