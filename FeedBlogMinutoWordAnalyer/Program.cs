using Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WordProcessor;
using WordProcessor.Models;

namespace FeedBlogMinutoWordAnalyer
{
    internal class Program
    {
        private static readonly IEnumerable<string> listOfWordToIgnore = new List<string>()
        {
            "a", "o", "as", "os", "para", "pra", "pro", "na", "nas", "no", "nos", "de", "do", "da", "das", "dos", "em", "por", "à",
            //"Gostou", "da", "matéria?", "Deixe", "sua", "nota!\n"
        };

        private static void Main(string[] args)
        {
            try
            {
                // Read the XML feed
                XmlProvider xmlProvider = new XmlProvider();
                var rssFeed = xmlProvider.Deserialize("Sources/FeedBlogMinutoSeguros.xml");

                // Get the text from each article and process it
                HtmlProvider htmlParser = new HtmlProvider();
                WordCounter wordCounter = new WordCounter();
                List<WordOccurrence> wordCountingList = new List<WordOccurrence>();
                Dictionary<string, IEnumerable<WordOccurrence>> wordCountingPerPage = new Dictionary<string, IEnumerable<WordOccurrence>>();
                foreach (var item in rssFeed.channel.item)
                {
                    Console.WriteLine(string.Format("Processing... {0} [{1}]", item.title, item.link));
                    Console.WriteLine(htmlParser.GetTextFromWebPage(item.link));
                    IEnumerable<WordOccurrence> wordCounting = wordCounter.CountWordOccurrences(htmlParser.GetTextFromWebPage(item.link), listOfWordToIgnore);
                    wordCountingList.AddRange(wordCounting);
                    wordCountingPerPage.Add(item.title, wordCounting);
                }

                // Output the results
                Console.WriteLine("\n\n\n\n\n");
                List<WordOccurrence> tenMostUsedWords = PrintTenMostUsedWords(wordCountingList);

                Console.WriteLine("\n");
                Console.WriteLine("Word count per article: \n");
                PrintOccurrancesPerPage(wordCountingPerPage, tenMostUsedWords);
                Console.ReadLine();
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private static List<WordOccurrence> PrintTenMostUsedWords(List<WordOccurrence> wordCountingList)
        {
            Console.WriteLine("The 10 most used word: \n");
            Console.WriteLine("\tWORD" + "".PadRight(21, '.') + "COUNT");
            Console.WriteLine("\n");

            var tenMostUsedWords = wordCountingList
                .GroupBy(g => new { g.Word, g.Count })
                .Select(wc => new WordOccurrence()
                {
                    Word = wc.Key.Word,
                    Count = wc.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            tenMostUsedWords
                .ForEach(y => Console.WriteLine("\t" + y.Word.PadRight(20, '.') + y.Count));

            return tenMostUsedWords;
        }

        private static void PrintOccurrancesPerPage(Dictionary<string, IEnumerable<WordOccurrence>> wordCountingPerPage, List<WordOccurrence> tenMostUsedWords)
        {
            foreach (var page in wordCountingPerPage)
            {
                foreach (var keyOccurrence in page.Value)
                {
                    if (tenMostUsedWords.Any(x => x.Word == keyOccurrence.Word))
                    {
                        Console.WriteLine("\t" + page.Key.PadRight(85, '.') + keyOccurrence.Word + " appeared " + keyOccurrence.Count + (keyOccurrence.Count == 1 ? " time" : " times"));
                    }
                }
            }
        }
    }
}
