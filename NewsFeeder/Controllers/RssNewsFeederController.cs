using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsFeeder.Models;
using NewsFeeder.Utilities;

namespace NewsFeeder.Controllers
{
	public class RssNewsFeederController : Controller
	{
		private readonly RssSettings _options;
		private readonly ILogger<RssNewsFeederController> _logger;

		public RssNewsFeederController(ILogger<RssNewsFeederController> logger, IOptions<RssSettings> options)
		{
			_logger = logger;
			_options = options.Value;
		}

		public IActionResult Index(bool isFormated = false)
		{
			var xDoc = XDocument.Load(_options.FeedUrl);

			var items = (from item in xDoc.Descendants("item")
						 select new
						 {
							 title = item.Element("title").Value,
							 link = item.Element("link").Value,
							 pubData = item.Element("pubDate").Value,
							 description = item.Element("description").Value
						 });

			List<RssNewsFeederModel> _feeds = new List<RssNewsFeederModel>();

			if (items == null)
			{
				_logger.LogWarning("Новости не получены.");
				return View(new RssModel { Feeders = _feeds, IsFormated = isFormated });
			}

			foreach (var item in items)
			{
				RssNewsFeederModel rssFeed = new RssNewsFeederModel
				{
					Title = item.title,
					Link = item.link,
					PublicationDate = DateUtilities.FormateDate(item.pubData),
					Description = isFormated
						? item.description
						: HtmlUtilities.ConvertToPlainText(item.description),
				};

				_feeds.Add(rssFeed);
			}

			return View(new RssModel { Feeders = _feeds, IsFormated = isFormated });
		}
	}
}
