using System;
using System.Collections.Generic;
using System.Linq;
using WordProcessor.Models;

namespace WordProcessor
{
    public class WordCounter
    {
        public IEnumerable<WordOccurrence> CountWordOccurrences(string s, IEnumerable<string> wordsToIgnore = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("message", nameof(s));
            }

            if (wordsToIgnore == null)
            {
                wordsToIgnore = Enumerable.Empty<string>();
            }

            string str = s;

            var result = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(w => !wordsToIgnore.Contains(w))
                            .GroupBy(r => r)
                            .Select(grp => new WordOccurrence
                            {
                                Word = grp.Key,
                                Count = grp.Count()
                            });

            return result;
        }


    }
}
