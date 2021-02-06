using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Models;

namespace Logic.Utils
{
    public sealed class Dispatcher
    {
        private readonly IServiceProvider _provider;

        public Dispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }


        public Task<Result> Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command is null)
                throw new ArgumentNullException();

            var commandType = command.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            var handler = (ICommandHandler<TCommand>)_provider.GetService(handlerType);
            if (handler == null)
                throw new Exception();

          return handler.Handle(command);
        }

        public async Task<T> Dispatch<T>(IQuery<T> query)
        {
            if (query is null)
                throw new ArgumentNullException();

            var queryType = query.GetType();
            var responseType = typeof(T);
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, responseType);

            dynamic handler = _provider.GetService(handlerType);
            T result = await handler.Handle((dynamic)query);
            return result;
        }
    }
}
