using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper
{
    public static class WordSearcher
    {
        public static IEnumerable<string> Search(KnownWords knownWords, UnknownWord unknownWord, List<string> availableLetters)
        {
            var wordPool = knownWords.AllWordsOfLength(unknownWord.WordPattern.Length);
            var potentialMatches = new HashSet<string>();
            if (wordPool.Count == 0)
            {
                return potentialMatches;
            }

            foreach (var replacementLetters in Permutations.OfSampleSize(availableLetters, unknownWord.UnknownCharCount))
            {
                var subWords = unknownWord.FillInUnknown(replacementLetters);
                if (subWords.All(word => wordPool.Contains(word)))
                {
                    var newWord = string.Join(" ", subWords);
                    potentialMatches.Add(newWord);
                }
            }

            return potentialMatches.OrderBy(w => w).ToList();
        }
    }
}
