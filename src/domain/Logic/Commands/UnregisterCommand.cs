using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Repositories;
using System;
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
            private readonly IGenericRepository<Student> _studentRepository;

            public UnregisterCommandHandler(IGenericRepository<Student> studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public Type CommandType => typeof(UnregisterCommand);

            public async Task<Result> Handle(UnregisterCommand command)
            {
                var  studentResult = await _studentRepository.Get(command.Id);
                if (studentResult.HasNoValue)
                    return Result.Failure($"No student found for Id {command.Id}");

                await _studentRepository.Delete(studentResult.Value.Id);
                await _studentRepository.Save();

                return Result.Success();
            }
        }
    }
}