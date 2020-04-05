using System.Threading.Tasks;
using AuctionAnalyserServer.Base.CQRS.Command;
using AuctionAnalyserServer.Base.CQRS.Query;

namespace AuctionAnalyserServer.Base.CQRS.Mediator
{
    public interface ICqrsMediator
    {
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;
        TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}
