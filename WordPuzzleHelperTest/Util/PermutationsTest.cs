using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using NUnit.Framework;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelperTest.Util
{
    [TestFixture]
    public class PermutationsTest
    {
        [Test]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void PermutationNull()
        {
            int[] testArr = null;
            Assert.Throws<ArgumentNullException>(() => Permutations.OfSampleSize(testArr, 1).ToList(), "Its expected that a null array is not a valid argument");
        }

        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void PermutationEmpty()
        {
            var testArr = Array.Empty<int>();
            Assert.Throws<ArgumentException>(() => Permutations.OfSampleSize(testArr, 1).ToList(),"Its expected that an empty array is not a valid argument");
        }

        [Test]
        [TestCaseSource(nameof(_PermutationTestDataHappyPath))]
        public void PermutationOfSampleSizeHappyPath(int[] someList, int ofSampleSize, int[][] expected)
        {
            var actual = Permutations.OfSampleSize(someList, ofSampleSize);
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(_PermutationTestDataArgumentException))]
        public void PermutationOfSampleSizeArgumentException(int[] someList, int ofSampleSize)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => Permutations.OfSampleSize(someList, ofSampleSize).ToList());
        }


        [Test]
        [TestCaseSource(nameof(_CartesianProductTestData))]
        public void CartesianProductTest(IEnumerable<IEnumerable<int>> listOfLists, IEnumerable<IEnumerable<int>> expectedResult)
        {
            var cartesian = Permutations.CartesianProduct(listOfLists);
            if (cartesian == null)
            {
                Assert.That(cartesian, Is.EqualTo(expectedResult));
            }
            else
            {
                Assert.That(cartesian, Is.EquivalentTo(expectedResult));
            }
        }

        private static IEnumerable<TestCaseData> _CartesianProductTestData()
        {
            {
                IEnumerable<IEnumerable<int>> listOfLists = null;
                IEnumerable<IEnumerable<int>> expectedResult = null;
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("null list");
            }

            {
                IEnumerable<IEnumerable<int>> listOfLists = new[] {new int[0]};
                IEnumerable<IEnumerable<int>> expectedResult = Enumerable.Empty<IEnumerable<int>>();
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("empty list");
            }

            {
                IEnumerable<IEnumerable<int>> listOfLists = new[] { new[]{ 5} };
                IEnumerable<IEnumerable<int>> expectedResult = new[] { new[] { 5 } };
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("1 single element list");
            }

            {
                IEnumerable<IEnumerable<int>> listOfLists = new[] { new[] { 1, 2, 3 } };
                IEnumerable<IEnumerable<int>> expectedResult = new[]
                {
                    new[] { 1 },
                    new[] { 2 },
                    new[] { 3 },
                };
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("1 multi element list");
            }

            {
                IEnumerable<IEnumerable<int>> listOfLists = new[]
                {
                    new[] { 1, 2, 3 },
                    new[] { 40, 50 }
                };
                IEnumerable<IEnumerable<int>> expectedResult = new[]
                {
                    new[] { 1, 40 },
                    new[] { 1, 50 },
                    new[] { 2, 40 },
                    new[] { 2, 50 },
                    new[] { 3, 40 },
                    new[] { 3, 50 },
                };
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("2 multi element lists");
            }

            {
                IEnumerable<IEnumerable<int>> listOfLists = new[]
                {
                    new[] { 1, 2, 3 },
                    new[] { 40, 50 },
                    new[] { 600, 700, 800 }
                };
                IEnumerable<IEnumerable<int>> expectedResult = new[]
                {
                    new[] { 1, 40, 600 },
                    new[] { 1, 40, 700 },
                    new[] { 1, 40, 800 },
                    new[] { 1, 50, 600 },
                    new[] { 1, 50, 700 },
                    new[] { 1, 50, 800 },
                    new[] { 2, 40, 600 },
                    new[] { 2, 40, 700 },
                    new[] { 2, 40, 800 },
                    new[] { 2, 50, 600 },
                    new[] { 2, 50, 700 },
                    new[] { 2, 50, 800 },
                    new[] { 3, 40, 600 },
                    new[] { 3, 40, 700 },
                    new[] { 3, 40, 800 },
                    new[] { 3, 50, 600 },
                    new[] { 3, 50, 700 },
                    new[] { 3, 50, 800 },
                };
                yield return new TestCaseData(listOfLists, expectedResult)
                    .SetName("3 multi element lists");
            }
        }

        private static IEnumerable<TestCaseData> _PermutationTestDataHappyPath()
        {
            {
                var someList = new[] { 1 };
                var ofSampleSize = 1;
                var expected = new[] { new[] { 1 } };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 1 element, ofSampleSize 1");
            }

            {
                var someList = new[] { 1, 2 };
                var ofSampleSize = 1;
                var expected = new[]
                {
                    new[] { 1 },
                    new[] { 2 },
                };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 2 elements, ofSampleSize 1");
            }

            {
                var someList = new[] { 1, 2 };
                var ofSampleSize = 2;
                var expected = new[]
                {
                    new[] { 1, 2 },
                    new[] { 2, 1 }
                };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 2 elements, ofSampleSize 2");
            }

            {
                var someList = new[] { 1, 2, 3 };
                var ofSampleSize = 1;
                var expected = new[]
                {
                    new[] { 1 },
                    new[] { 2 },
                    new[] { 3 },
                };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 3 elements, ofSampleSize 1");
            }

            {
                var someList = new[] { 1, 2, 3 };
                var ofSampleSize = 2;
                var expected = new[]
                {
                    new[] { 1, 2 },
                    new[] { 1, 3 },
                    new[] { 2, 1 },
                    new[] { 2, 3 },
                    new[] { 3, 1 },
                    new[] { 3, 2 },
                };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 3 elements, ofSampleSize 2");
            }

            {
                var someList = new[] { 1, 2, 3 };
                var ofSampleSize = 3;
                var expected = new[]
                {
                    new[] { 1, 2, 3 },
                    new[] { 1, 3, 2 },
                    new[] { 2, 1, 3 },
                    new[] { 2, 3, 1 },
                    new[] { 3, 1, 2 },
                    new[] { 3, 2, 1 },
                };
                yield return new TestCaseData(someList, ofSampleSize, expected)
                    .SetName("someList 3 elements, ofSampleSize 3");
            }
        }

        private static IEnumerable<TestCaseData> _PermutationTestDataArgumentException()
        {
            {
                var someList = new[] { 1, 3, 4 };
                var ofSampleSize = -100;
                yield return new TestCaseData(someList, ofSampleSize).SetName("ofSampleSize -100");
            }

            {
                var someList = new[] { 1, 3, 4 };
                var ofSampleSize = 0;
                yield return new TestCaseData(someList, ofSampleSize).SetName("ofSampleSize 0");
            }

            {
                var someList = new[] { 1, 3, 4 };
                var ofSampleSize = 5;
                yield return new TestCaseData(someList, ofSampleSize).SetName("someList 3 elements, ofSampleSize 5");
            }
        }

    }
}
