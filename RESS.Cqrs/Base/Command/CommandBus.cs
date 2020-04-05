using Autofac;
using System;

namespace AuctionAnalyserServer.Base.CQRS.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly IComponentContext _context;

        public CommandBus(IComponentContext componenet)
        {
            _context = componenet;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command),
                    $"Command: '{typeof(TCommand).Name}' can not be null.");
            }

            var handler = _context.Resolve<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }
    }
}
