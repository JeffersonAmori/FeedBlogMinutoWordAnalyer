using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Providers
{
    public class HtmlProvider
    {
        public string GetTextFromWebPage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("message", nameof(url));
            }

            var html = url;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var textNodes = htmlDoc.DocumentNode.SelectNodes("//div/p").ToList();

            return string.Concat(
                textNodes
                    .Take(textNodes.Count - 3)
                    .Select(node => Regex.Replace(node.InnerHtml.ToUpper(), "<.*?>", string.Empty)));
        }
    }
}
