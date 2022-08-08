using System.IO;
using HtmlAgilityPack;

namespace NewsFeeder.Utilities
{
	public class HtmlUtilities
	{
		/// <summary>
		/// Преобразует HTML в обычный текст / удаляет теги.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <returns></returns>
		public static string ConvertToPlainText(string html)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);

			StringWriter sw = new StringWriter();
			ConvertTo(doc.DocumentNode, sw);
			sw.Flush();
			return sw.ToString();
		}


		/// <summary>
		/// Подсчитайте слова.
		/// Перед этим содержимое должно быть преобразовано в обычный текст (с помощью ConvertToPlainText).
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <returns></returns>
		public static int CountWords(string plainText)
		{
			return !string.IsNullOrEmpty(plainText) ? plainText.Split(' ', '\n').Length : 0;
		}


		public static string Cut(string text, int length)
		{
			if (!string.IsNullOrEmpty(text) && text.Length > length)
			{
				text = text.Substring(0, length - 4) + " ...";
			}
			return text;
		}


		private static void ConvertContentTo(HtmlNode node, TextWriter outText)
		{
			foreach (HtmlNode subnode in node.ChildNodes)
			{
				ConvertTo(subnode, outText);
			}
		}


		private static void ConvertTo(HtmlNode node, TextWriter outText)
		{
			string html;
			switch (node.NodeType)
			{
				// не выводить комментарии
				case HtmlNodeType.Comment:
					break;

				case HtmlNodeType.Document:
					ConvertContentTo(node, outText);
					break;

				case HtmlNodeType.Text:
					// сценарий и стиль не должны выводиться
					string parentName = node.ParentNode.Name;
					if (parentName == "script" || parentName == "style")
						break;

					// получить текст
					html = ((HtmlTextNode)node).Text;

					// действительно ли это специальный закрывающий узел, выводимый как текст?
					if (HtmlNode.IsOverlappedClosingElement(html))
						break;

					// проверьте, чтобы текст был осмысленным, а не состоял из пробелов
					if (html.Trim().Length > 0)
					{
						outText.Write(HtmlEntity.DeEntitize(html));
					}
					break;

				case HtmlNodeType.Element:
					switch (node.Name)
					{
						// рассматривать абзацы как crlf
						case "p":
							outText.Write("\r\n");
							break;
						case "br":
							outText.Write("\r\n");
							break;
					}

					if (node.HasChildNodes)
					{
						ConvertContentTo(node, outText);
					}
					break;
			}
		}
	}
}
