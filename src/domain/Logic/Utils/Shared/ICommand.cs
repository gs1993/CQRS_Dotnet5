using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace Logic.Utils.Shared
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