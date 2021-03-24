using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Utils;

namespace Logic.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DatabaseRetryAttribute : Attribute
    {
        public DatabaseRetryAttribute()
        {
        }
    }

    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly DatabaseSettings _config;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, DatabaseSettings config)
        {
            _config = config;
            _handler = handler;
        }

        public Type CommandType => throw new NotImplementedException();

        public async Task<Result> Handle(TCommand command)
        {
            for (int i = 0; ; i++)
            {
                try
                {
                    Result result = await _handler.Handle(command);
                    return result;
                }
                catch (Exception ex)
                {
                    if (i >= _config.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }


        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;
            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }
}
