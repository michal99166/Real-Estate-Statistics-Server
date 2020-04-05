using System;

namespace AuctionAnalyserServer.Base.CQRS.Command
{
    public interface IAuthenticatedCommand :  ICommand
    {
        Guid UserId { get; set; }
    }
}