using System.Collections.Generic;
using System.Linq;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelper.Puzzle
{
    public class CrosswordPuzzle : BasePuzzle
    {
        public CrosswordPuzzle(KnownWords knownWords)
            : base(knownWords)
        {
        }

        public IEnumerable<string> Solve(UnknownWord unknownWord, int[] subWordCount)
        {
            // call SetSubWordCounts just for the validation it does
            unknownWord.SetSubWordCounts(subWordCount);
            string[] wordsToSolve = {unknownWord.WordPattern};
            if (subWordCount != null)
            {
                wordsToSolve = new string[subWordCount.Length];
                var pattern = unknownWord.WordPattern;
                var currentIdx = 0;
                for(var i = 0; i<subWordCount.Length; i++)
                {
                    var wordCount = subWordCount[i];
                    var subWord = pattern.Substring(currentIdx, wordCount);
                    wordsToSolve[i] = subWord;
                    currentIdx += wordCount;
                }
            }
            else
            {
                subWordCount = new []{unknownWord.WordPattern.Length};
            }

            var matches = new List<string>[wordsToSolve.Length];
            for (var i = 0; i < matches.Length; i++)
            {
                var allWordsOfLen = KnownWords.AllWordsOfLength(subWordCount[i]);
                matches[i] = new List<string>();
                foreach (var word in allWordsOfLen)
                {
                    if (_IsMatch(wordsToSolve[i], word))
                    {
                        matches[i].Add(word);
                    }
                }
            }

            foreach (var wordList in matches)
            {
                wordList.Sort();
            }

            var allPossiblePhrases = Permutations.CartesianProduct(matches);
            var formattedPhrases = allPossiblePhrases.Select(phrase => string.Join(" ", phrase));
            return formattedPhrases.ToList();
        }

        private bool _IsMatch(string unknownWord, string word)
        {
            for (var i = 0; i < word.Length; i++)
            {
                if (unknownWord[i] == UnknownWord.UnknownToken)
                {
                    continue;
                }

                if (unknownWord[i] != word[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
