using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleLib
{
    public static class WordSearcher
    {
        public static IEnumerable<string> Search(KnownWords knownWords, UnknownWord unknownWord, List<string> availableLetters)
        {
            if (_AllUnknownWordLengthsReasonable(knownWords, unknownWord) == false)
            {
                return new HashSet<string>();
            }

            var potentialMatches = new HashSet<string>();
            foreach (var replacementLetters in Permutations.OfSampleSize<string>(availableLetters, unknownWord.UnknownCharCount))
            {
                var subWords = unknownWord.FillInUnknown(replacementLetters);
                if (subWords.All(knownWords.IsKnownWord))
                {
                    var newWord = string.Join(" ", subWords);
                    potentialMatches.Add(newWord);
                }
            }

            return potentialMatches.OrderBy<string, string>(w => w).ToList();
        }

        private static bool _AllUnknownWordLengthsReasonable(KnownWords knownWords, UnknownWord unknownWord)
        {
            if (unknownWord.SubWordCounts == null)
            {
                if (knownWords.AllWordsOfLength(unknownWord.WordPattern.Length).Count == 0)
                {
                    return false;
                }
            }
            else
            {
                if (unknownWord.SubWordCounts.Any(cnt => knownWords.AllWordsOfLength(cnt).Count == 0))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
