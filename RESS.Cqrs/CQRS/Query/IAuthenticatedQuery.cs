using System;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public interface IAuthenticatedQuery : IQuery
    {
        Guid UserId { get; set; }

    }
}