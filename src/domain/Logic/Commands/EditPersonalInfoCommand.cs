using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Models;
using Logic.Repositories;
using System.Threading.Tasks;

namespace Logic.Commands
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
            private readonly IGenericRepository<Student> _studentRepository;

            public EditPersonalInfoCommandHandler(IGenericRepository<Student> studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public async Task<Result> Handle(EditPersonalInfoCommand command)
            {
                var studentResult = await _studentRepository.TryGet(command.Id);
                if (studentResult.HasNoValue)
                    return Result.Fail($"No student found for Id {command.Id}");

                var student = studentResult.Value;
                student.Name = command.Name;
                student.Email = command.Email;

                await _studentRepository.Save();

                return Result.Ok();
            }
        }
    }
}
