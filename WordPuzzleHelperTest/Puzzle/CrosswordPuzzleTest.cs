using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzleHelper;
using WordPuzzleHelper.Puzzle;

namespace WordPuzzleHelperTest.Puzzle
{
    [TestFixture]
    public class CrosswordPuzzleTest
    {
        [Test]
        public void UnknownTokenKnownForTesting()
        {
            // I want to be able to use the token directly in strings here because it makes whats being tested less verbose, meaning easier to
            // to follow read tests. 
            Assert.That('?', Is.EqualTo(UnknownWord.UnknownToken), "The UnknownToken has changed, be sure to change all the tests in this file that assumed what the token was");
        }

        [Test]
        [TestCaseSource(nameof(_CrosswordData))]
        public void SolveTest(KnownWords knownWords, UnknownWord unknownWord, int[] subWordCount, string expectedWord)
        {
            var crossword = new CrosswordPuzzle(knownWords);
            var solutions = crossword.Solve(unknownWord, subWordCount);
            Assert.That(solutions, Has.Member(expectedWord), "expected word was not in the list of solutions");
        }

        private static IEnumerable<TestCaseData> _CrosswordData()
        {
            {
                var knownWords = new KnownWords(new[]{"one", "two", "ane", "ene", "ine"});
                var unknownWord = new UnknownWord("?ne");
                int[] subWordCount = null;
                var expectedWord = "one";
                yield return new TestCaseData(knownWords, unknownWord, subWordCount, expectedWord)
                    .SetName("word: something");
            }

            {
                var knownWords = new KnownWords(new[] { "one", "two", "ane", "ene", "ine" });
                var unknownWord = new UnknownWord("?net??");
                int[] subWordCount = {3, 3};
                var expectedWord = "one two";
                yield return new TestCaseData(knownWords, unknownWord, subWordCount, expectedWord)
                    .SetName("word: one two");
            }

            {
                var knownWords = new KnownWords(new[] { "teen", "ween", "town", "then" });
                var unknownWord = new UnknownWord("t??n");
                int[] subWordCount = null;
                var expectedWord = "teen";
                yield return new TestCaseData(knownWords, unknownWord, subWordCount, expectedWord)
                    .SetName("word: teen");
            }
        }
    }
}
