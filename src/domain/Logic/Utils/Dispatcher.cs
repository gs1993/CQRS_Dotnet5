using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Models;

namespace Logic.Utils
{
    public sealed class Dispatcher
    {
        private readonly IServiceProvider _provider;
        private readonly ICommandHandlerExecutor _commandHandlerExecutor;

        public Dispatcher(IServiceProvider provider, 
            ICommandHandlerExecutor commandHandlerExecutor)
        {
            _provider = provider;
            _commandHandlerExecutor = commandHandlerExecutor;
        }


        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }

        public Task<Result> DispatchAsync(ICommand command)
        {
            return _commandHandlerExecutor.Execute(command);
        }

        public T Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            T result = handler.Handle((dynamic)query);

            return result;
        }

        public async Task<T> DispatchAsync<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            T result = await handler.Handle((dynamic)query);

            return result;
        }
    }
}
