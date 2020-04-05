using System.Threading.Tasks;
using AuctionAnalyserServer.Base.CQRS.Command;
using AuctionAnalyserServer.Base.CQRS.Query;

namespace AuctionAnalyserServer.Base.CQRS.Mediator
{
    public interface ICqrsMediatorAsync
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}