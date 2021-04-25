using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Decorators.Command;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Students.Commands
{
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public long Id { get; }
        public string Name { get; }
        public string Email { get; }

        public EditPersonalInfoCommand(long id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
        {
            private readonly IRepository _repository;

            public EditPersonalInfoCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(EditPersonalInfoCommand);

            public async Task<Result> Handle(EditPersonalInfoCommand command)
            {
                var student = await _repository.GetByIdAsync<Student>(command.Id);
                if (student == null)
                    return Result.Failure($"No student found for Id {command.Id}");

                student.Name = command.Name;
                student.Email = command.Email;

                await _repository.UpdateAsync(student);

                return Result.Success();
            }
        }
    }
}
