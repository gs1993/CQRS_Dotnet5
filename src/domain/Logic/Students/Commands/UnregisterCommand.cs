using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Students.Commands
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
            private readonly IRepository _repository;

            public UnregisterCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(UnregisterCommand);

            public async Task<Result> Handle(UnregisterCommand command)
            {
                var student = await _repository.GetByIdAsync<Student>(command.Id);
                if (student == null)
                    return Result.Failure($"No student found for Id {command.Id}");

                await _repository.DeleteAsync(student);

                return Result.Success();
            }
        }
    }
}