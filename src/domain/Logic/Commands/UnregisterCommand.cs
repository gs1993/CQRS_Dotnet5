using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Repositories;
using Logic.Students;
using Logic.Utils;
using System.Threading.Tasks;

namespace Logic.Commands
{
    public sealed class UnregisterCommand : ICommand
    {
        public long Id { get; }

        public UnregisterCommand(long id)
        {
            Id = id;
        }

        internal sealed class UnregisterCommandHandler : ICommandHandler<UnregisterCommand>
        {
            


            public async Task<Result> Handle(UnregisterCommand command)
            {
                Student student = repository.GetById(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                repository.Delete(student);
                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}