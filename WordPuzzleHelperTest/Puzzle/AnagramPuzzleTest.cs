using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WordPuzzleHelper;
using WordPuzzleHelper.Puzzle;

namespace WordPuzzleHelperTest.Puzzle
{
    [TestFixture]
    public class AnagramPuzzleTest
    {
        [Test]
        public void UnknownTokenKnownForTesting()
        {
            // I want to be able to use the token directly in strings here because it makes whats being tested less verbose, meaning easier to
            // to follow read tests. 
            Assert.That('?', Is.EqualTo(UnknownWord.UnknownToken), "The UnknownToken has changed, be sure to change all the tests in this file that assumed what the token was");
        }

        [Test]
        public void WordCannotHaveUnknowns()
        {
            var word = "wh?t";
            var subject = new AnagramPuzzle(new KnownWords(Enumerable.Empty<string>()));
            Assert.Throws<ArgumentException>(() => subject.Solve(word));
            word = word.Replace(UnknownWord.UnknownToken, 'a');
            Assert.DoesNotThrow(() => subject.Solve(word), "removing the unknown character should have fixed issue of throwing ArgumentException");
        }

        [Test]
        public void AnagramNeedsAtLeastTwoLetters()
        {
            var word = "A";
            var subject = new AnagramPuzzle(new KnownWords(Enumerable.Empty<string>()));
            Assert.Throws<ArgumentException>(() => subject.Solve(word));
            word = "AA";
            Assert.DoesNotThrow(() => subject.Solve(word), "Having at least 2 characters should have fixed issue of throwing ArgumentException");
        }

        [Test]
        public void AnagramsDoNotIncludeOriginalWord()
        {
            var word = "decorations";
            var knownWords = new KnownWords(new[] { "decorations", "coordinates" });
            var subject = new AnagramPuzzle(knownWords);
            var anagrams = subject.Solve(word);
            var expected = new[] { "coordinates" };
            Assert.That(anagrams, Is.EquivalentTo(expected), "solved anagrams should not include the original word");
        }

        [Test]
        [TestCaseSource(nameof(_AnagramTestData))]
        public void SolveTest(KnownWords knownWords, string originalWord, string expectedAnagram)
        {
            var puzzle = new AnagramPuzzle(knownWords);
            var solutions = puzzle.Solve(originalWord);
            Assert.That(solutions, Has.Member(expectedAnagram), "expected anagram was not in the list of solutions");
        }

        private static IEnumerable<TestCaseData> _AnagramTestData()
        {
            // snagged a few easy anagrams from wikipedia
            var knownWords = new KnownWords(new[]{"evil", "funeral", "adultery", "totems", "lie", "mistletoe" });

            {
                var originalWord = "vile";
                var expectedWord = "evil";
                yield return new TestCaseData(knownWords, originalWord, expectedWord)
                    .SetName("vile->evil");
            }

            {
                var originalWord = "realFun";
                var expectedWord = "funeral";
                yield return new TestCaseData(knownWords, originalWord, expectedWord)
                    .SetName("realFun->funeral");
            }

            {
                var originalWord = "trueLady";
                var expectedWord = "adultery";
                yield return new TestCaseData(knownWords, originalWord, expectedWord)
                    .SetName("trueLady->adultery");
            }

            {
                var originalWord = "totems lie";
                var expectedWord = "mistletoe";
                yield return new TestCaseData(knownWords, originalWord, expectedWord)
                    .SetName("totems lie->mistletoe");
            }
        }
    }
}
