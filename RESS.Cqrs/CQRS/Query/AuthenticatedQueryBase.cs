using System;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public class AuthenticatedQueryBase : IAuthenticatedQuery
    {
        public Guid UserId { get; set; }

    }
}