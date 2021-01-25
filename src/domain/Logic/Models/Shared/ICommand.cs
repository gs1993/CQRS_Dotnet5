using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Logic.Models
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command);
    }


}