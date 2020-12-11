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
        public void AnagramNeedsAtLeastTwoLetters()
        {
            var word = "A";
            var subject = new AnagramPuzzle(new KnownWords(Enumerable.Empty<string>()));
            Assert.Throws<ArgumentException>(() => subject.Solve(word));
        }

        [Test]
        [TestCaseSource(nameof(_AnagramTestData))]
        public void SolveTest(KnownWords knownWords, string originalWord, string expectedWord)
        {
            var puzzle = new AnagramPuzzle(knownWords);
            var solutions = puzzle.Solve(originalWord);
            Assert.That(solutions, Has.Member(expectedWord), "expected word was not in the list of solutions");
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
