using Autofac;
using System;
using System.Threading.Tasks;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public class QueryBusAsync : IQueryBusAsync
    {
        private readonly IComponentContext _context;

        public QueryBusAsync(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query),
                    $"Query: '{typeof(TQuery).Name}' can not be null.");
            }

            var handler = _context.Resolve<IQueryHandlerAsync<TQuery, TResult>>();
            return await handler.ExecuteAsync(query);
        }
    }
}
