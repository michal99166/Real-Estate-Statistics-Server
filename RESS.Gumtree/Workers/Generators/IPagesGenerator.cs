using System.Collections.Generic;

namespace RESS.Gumtree.Workers.Generators
{
    public interface IPagesGenerator
    {
        IEnumerable<PageData> BuildPagesUrls();
    }
}