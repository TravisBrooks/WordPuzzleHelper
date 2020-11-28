using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper
{
    public static class WordSearcher
    {
        public static IEnumerable<string> Search(KnownWords knownWords, UnknownWord unknownWord, List<string> availableLetters)
        {
            // This word search algorithm is more difficult than necessary because it takes the availableLetters param. Most word puzzles
            // don't have a funny alphabet or a fixed pool of known letters (like scrabble) so this extra work is not necessary.
            // Contemplating a fast/simple version for the typical crossword style puzzle
            if (_AllUnknownWordLengthsReasonable(knownWords, unknownWord) == false)
            {
                return new HashSet<string>();
            }

            var potentialMatches = new HashSet<string>();
            foreach (var replacementLetters in Permutations.OfSampleSize(availableLetters, unknownWord.UnknownCharCount))
            {
                var subWords = unknownWord.FillInUnknown(replacementLetters);
                if (subWords.All(knownWords.IsKnownWord))
                {
                    var newWord = string.Join(" ", subWords);
                    potentialMatches.Add(newWord);
                }
            }

            return potentialMatches.OrderBy(w => w).ToList();
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
