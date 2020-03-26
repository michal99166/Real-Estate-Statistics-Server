using System.Collections.Generic;

namespace RESS.Gumtree.Workers.Generators
{
    public class PageData
    {
        public int StartInterval { get; set; }
        public int EndInterval { get; set; }
        public Dictionary<string, List<string>> PagesWithTopicUrls { get; set; }
    }
}