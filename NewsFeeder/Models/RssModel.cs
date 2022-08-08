using System.Collections.Generic;

namespace NewsFeeder.Models
{
	public class RssModel
	{
		public IEnumerable<RssNewsFeeder> Feeders { get; set; }

		public bool IsFormated { get; set; }
	}
}
