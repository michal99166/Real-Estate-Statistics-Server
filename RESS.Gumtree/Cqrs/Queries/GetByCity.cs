using Convey.CQRS.Queries;
using RESS.Gumtree.DTO;

namespace RESS.Gumtree.Cqrs.Queries
{
    public class GetByCity : PagedQueryBase, IQuery<PagedResult<GumtreeTopicDto>>
    {
        public string City { get; set; }
    }
}