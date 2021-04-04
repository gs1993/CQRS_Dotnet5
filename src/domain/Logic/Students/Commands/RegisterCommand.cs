using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Decorators.Command;
using Logic.Utils.Shared;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Students.Commands
{
    public sealed class RegisterCommand : ICommand
    {
        public string Name { get; }
        public string Email { get; }
        public string Course1 { get; }
        public string Course1Grade { get; }
        public string Course2 { get; }
        public string Course2Grade { get; }


        public RegisterCommand(string name, string email, string course1, string course1Grade, string course2, string course2Grade)
        {
            Name = name;
            Email = email;
            Course1 = course1;
            Course1Grade = course1Grade;
            Course2 = course2;
            Course2Grade = course2Grade;
        }


        [AuditLog]
        internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
        {
            private readonly IRepository _repository;

            public RegisterCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(RegisterCommand);

            public async Task<Result> Handle(RegisterCommand command)
            {
                var createSrudentResult = Student.Create(command.Name, command.Email);
                if (createSrudentResult.IsFailure)
                    return Result.Failure(createSrudentResult.Error);

                var student = createSrudentResult.Value;
                if (command.Course1 != null && command.Course1Grade != null)
                {
                    var course = await _repository.GetAsync<Course>(c => c.Name == command.Course1);
                    if (course == null)
                        return Result.Failure($"Course '{command.Course1}' not found");

                    student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
                }

                if (command.Course2 != null && command.Course2Grade != null)
                {
                    var course = await _repository.GetAsync<Course>(c => c.Name == command.Course2);
                    if (course == null)
                        return Result.Failure($"Course '{command.Course2}' not found");

                    student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
                }

                var insertResult = await _repository.InsertAsync(student);

                return Result.Success();
            }
        }
    }
}