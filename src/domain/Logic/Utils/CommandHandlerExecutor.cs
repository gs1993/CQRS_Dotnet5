using CSharpFunctionalExtensions;
using Logic.Utils.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Utils
{
    public interface ICommandHandlerExecutor
    {
        Task<Result> Execute(ICommand command);
    }

    public class CommandHandlerExecutor : ICommandHandlerExecutor
    {
        private readonly IEnumerable<ICommandHandler<ICommand>> _commandHandlers;

        public CommandHandlerExecutor(IEnumerable<ICommandHandler<ICommand>> commandHandlers)
        {
            _commandHandlers = commandHandlers ?? throw new ArgumentNullException();
        }


        public Task<Result> Execute(ICommand command)
        {
            if (command is null)
                throw new ArgumentNullException();

            var commandType = command.GetType();
            var handler = _commandHandlers.FirstOrDefault(x => x.CommandType == commandType);
            if (handler == null)
                throw new Exception();

            return handler.Handle(command);
        }
    }
}
