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

			List<RssNewsFeeder> _feeds = new List<RssNewsFeeder>();

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
					RssNewsFeeder rssFeed = new RssNewsFeeder
					{
						Title = item.title,
						Link = item.link,
						PubDate = item.pubData,
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
