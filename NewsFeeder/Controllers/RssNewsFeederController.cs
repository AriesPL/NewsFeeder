using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using NewsFeeder.Models;

namespace NewsFeeder.Controllers
{
	public class RssNewsFeederController : Controller
	{
		public IActionResult Index(bool IsFormated = false)
		{
			string rssFeedUrl = "https://habr.com/ru/rss/interesting/";

			List<RssNewsFeederModel> _feeds = new List<RssNewsFeederModel>();

			XDocument xDoc = new XDocument();
			xDoc = XDocument.Load(rssFeedUrl);

			var items = (from x in xDoc.Descendants("item")
						 select new
						 {
							 title = x.Element("title").Value,
							 link = x.Element("link").Value,
							 pubData = x.Element("pubDate").Value,
							 description = x.Element("description").Value
						 });

			if (items != null)
			{
				foreach (var item in items)
				{
					RssNewsFeederModel rssFeed = new RssNewsFeederModel
					{
						Title = item.title,
						Link = item.link,
						PubDate = DateFormat.GetTime(item.pubData),
						Description = HtmlUtilities.ConvertToPlainText(item.description),
                        Description2 = item.description 
                    };

					_feeds.Add(rssFeed);
				}

			}
			return View(new RssModel { Feeders = _feeds, IsFormated = IsFormated } );
		}
	}
}
