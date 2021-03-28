using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Models;
using Newtonsoft.Json;

namespace Logic.Decorators.Command
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AuditLogAttribute : Attribute
    {
        public AuditLogAttribute() { }
    }

    public sealed class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            _handler = handler;
        }

        public Type CommandType => typeof(AuditLogAttribute);

        public Task<Result> Handle(TCommand command)
        {
            string commandJson = JsonConvert.SerializeObject(command);

            // Use proper logging here
            Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

            return _handler.Handle(command);
        }

    }
}
