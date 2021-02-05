using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace Logic.Models
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<ICommand> 
    {
        Type CommandType { get; }

        Task<Result> Handle(ICommand command);
    }
}