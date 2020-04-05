using System;

namespace AuctionAnalyserServer.Base.CQRS.Command
{
    public class AuthenticatedCommandBase : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
    }
}