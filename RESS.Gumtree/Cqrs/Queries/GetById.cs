using System;
using Convey.CQRS.Queries;
using RESS.Gumtree.DTO;

namespace RESS.Gumtree.Cqrs.Queries
{
    public class GetById : IQuery<GumtreeTopicDto>
    {
        public Guid Id { get; set; }
    }
}