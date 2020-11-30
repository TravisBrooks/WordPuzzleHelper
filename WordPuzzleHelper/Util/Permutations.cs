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

            if (sampleSize <= 0)
            {
                throw new ArgumentException($"sampleSize must be greater than 0, was {sampleSize}");
            }

            if (someList.Count < sampleSize)
            {
                throw new ArgumentException(
                    $"sampleSize cannot be greater than list length, sampleSize: {sampleSize}, list length: {someList.Count}");
            }

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
                    var frontOfList = someList.TakeWhile((t, i) => i < idx);
                    var tailOfList = someList.SkipWhile((t, i) => i <= idx);
                    foreach (var otherPerm in OfSampleSize(frontOfList.Concat(tailOfList).ToList(), sampleSize - 1))
                    {
                        var perm = new List<T> { someList[idx] };
                        perm.AddRange(otherPerm);
                        yield return perm;
                    }
                }
            }
        }
    }
}
