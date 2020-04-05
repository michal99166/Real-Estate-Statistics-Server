using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAnalyserServer.Base.CQRS.Command
{
    public interface ICommandBus
    {
        void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
