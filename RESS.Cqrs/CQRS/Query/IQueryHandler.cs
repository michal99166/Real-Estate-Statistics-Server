using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
    {
        TResult Execute(TQuery query);
    }
}
