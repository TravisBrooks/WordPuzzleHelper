using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper.Util
{
    public static class Permutations
    {
        public static IEnumerable<IList<T>> OfSampleSize<T>(IList<T> someList, int sampleSize)
        {
            if (someList == null)
            {
                throw new ArgumentNullException(nameof(someList));
            }

            if (someList.Any() == false)
            {
                throw new ArgumentException($"{nameof(someList)} cannot be empty.");
            }

            if (sampleSize <= 0)
            {
                throw new ArgumentException($"sampleSize must be greater than 0, was {sampleSize}");
            }

            if (someList.Count < sampleSize)
            {
                throw new ArgumentException(
                    $"sampleSize cannot be greater than list length, sampleSize: {sampleSize}, list length: {someList.Count}");
            }

            // this is a reference sort of implementation of how to do permutations, intended more for clarity and accuracy then speed
            if (sampleSize == 1)
            {
                foreach (var t in someList)
                {
                    yield return new[] { t };
                }
            }
            else
            {
                for (var idx = 0; idx < someList.Count; idx++)
                {
                    var frontOfList = someList.TakeWhile((t, i) => i < idx).ToList();
                    var tailOfList = someList.SkipWhile((t, i) => i <= idx).ToList();
                    foreach (var otherPerm in OfSampleSize(frontOfList.Concat(tailOfList).ToList(), sampleSize - 1))
                    {
                        var perm = new List<T> { someList[idx] };
                        perm.AddRange(otherPerm);
                        yield return perm;
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> listOfLists)
        {
            if (listOfLists == null)
            {
                return null;
            }
            IEnumerable<IEnumerable<T>> accumulator = new[] { Enumerable.Empty<T>() };
            foreach (var currentList in listOfLists)
            {
                accumulator = _CartesianProductLoop(currentList, accumulator);
            }
            return accumulator;
        }

        private static IEnumerable<IEnumerable<T>> _CartesianProductLoop<T>(IEnumerable<T> currentList, IEnumerable<IEnumerable<T>> accumulator)
        {
            // Yes, this method can all be done in 1 call to SelectMany, but I find the syntax of SelectMany to be more confusing to read
            var localAccumulator = new List<IEnumerable<T>>();
            foreach (var accList in accumulator)
            {
                foreach (var item in currentList)
                {
                    var accItem = accList.Concat(new[] {item});
                    localAccumulator.Add(accItem);
                }
            }
            return localAccumulator;
        }
    }
}
