using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    // TODO: not sure I even want to support this puzzle type since I don't really solve scrabble puzzles. Leaving this here as a placeholder

    //public class ScrabblePuzzle : BasePuzzle
    //{
    //    public ScrabblePuzzle(KnownWords knownWords) : base(knownWords)
    //    {
    //    }

    //    public IEnumerable<string> Solve(UnknownWord partialWordOrLength, char[] availableLetters)
    //    {
    //        var matches = _ScrabbleSearch(KnownWords, partialWordOrLength, availableLetters);
    //        // TODO: sort the words with the highest scrabble point value first
    //        // TODO: I'm not taking the blank tile into consideration. Its a wild card initially but after its value has been assigned once then all blank tiles have that same value.
    //        // TODO: The scrabble puzzle is pretty poorly served by whats going on here. Main issue being I don't really play scrabble that much. Maybe the word `scrabble` is wrong here???
    //        return matches;
    //    }
    //}

    //private static IEnumerable<string> _ScrabbleSearch(KnownWords knownWords, UnknownWord unknownWord, char[] availableLetters)
    //{
    //    if (_AllUnknownWordLengthsReasonable(knownWords, unknownWord) == false)
    //    {
    //        return new HashSet<string>();
    //    }

    //    var potentialMatches = new HashSet<string>();
    //    // This word search algorithm is more difficult than necessary because it takes the availableLetters param. Most word puzzles
    //    // don't have a funny alphabet or a fixed pool of known letters (like scrabble) so this extra work is not necessary.
    //    // Contemplating a fast/simple version for the typical crossword style puzzle
    //    // TODO: the permutation thing seems non-optimal. Instead should try to filter the known words.
    //    foreach (var replacementLetters in Permutations.OfSampleSize(availableLetters, unknownWord.UnknownCharCount))
    //    {
    //        var subWords = unknownWord.FillInUnknown(replacementLetters);
    //        if (subWords.All(knownWords.IsKnownWord))
    //        {
    //            var newWord = string.Join(" ", subWords);
    //            potentialMatches.Add(newWord);
    //        }
    //    }

    //    return potentialMatches.OrderBy(w => w).ToList();
    //}
}
