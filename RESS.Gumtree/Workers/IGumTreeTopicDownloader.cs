using System.Collections.Generic;
using System.Threading.Tasks;
using RESS.Gumtree.Workers.Generators;

namespace RESS.Gumtree.Workers
{
    public interface IGumTreeTopicDownloader
    {
        void GetTopicContentSynch(IEnumerable<PageData> pagesIntervalData);
    }
}