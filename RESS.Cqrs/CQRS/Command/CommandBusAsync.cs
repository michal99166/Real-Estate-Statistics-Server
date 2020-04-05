using Autofac;
using System;
using System.Threading.Tasks;

namespace AuctionAnalyserServer.Base.CQRS.Command
{
    public class CommandBusAsync : ICommandBusAsync
    {
        private readonly IComponentContext _context;

        public CommandBusAsync(IComponentContext context)
        {
            _context = context;
        }

        public async Task ExecuteCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command),
                    $"Command: '{typeof(TCommand).Name}' can not be null.");
            }

            var handler = _context.Resolve<ICommandHandlerAsync<TCommand>>();
            await handler.HandleAsync(command);
        }
    }
}
