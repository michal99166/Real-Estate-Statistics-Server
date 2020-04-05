using Autofac;
using System;

namespace AuctionAnalyserServer.Base.CQRS.Query
{
    public class QueryBus : IQueryBus
    {
        private readonly IComponentContext _context;

        public QueryBus(IComponentContext context)
        {
            _context = context;
        }

        public TResult ExecuteQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query),
                    $"Query: '{typeof(TQuery).Name}' can not be null.");
            }

            var handler = _context.Resolve<IQueryHandler<TQuery, TResult>>();
            return handler.Execute(query);
        }
    }
}
