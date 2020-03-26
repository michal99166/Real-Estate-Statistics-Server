using System.Collections.Generic;

namespace RESS.Gumtree.Workers.Generators
{
    public static class MaxPagesGenerator
    {
        private const int MaxPages = 50;

        public static IEnumerable<string> BuildUrlsWithMaxPagesNumber(string baseUrl)
        {
            for (int i = 1; i <= MaxPages; i++)
            {
                yield return $"{baseUrl.Replace("p1", $"p{i}")}";
            }
        }
    }
}