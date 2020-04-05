using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public interface IQueryBusAsync
    {
        Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}
