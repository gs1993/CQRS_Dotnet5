using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Models;
using Logic.Repositories;

namespace Logic.Commands
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
            private readonly IGenericRepository<Student> _studentRepository;
            private readonly ICourseRepository _courseRepository;

            public RegisterCommandHandler(IGenericRepository<Student> studentRepository, ICourseRepository courseRepository)
            {
                _studentRepository = studentRepository;
                _courseRepository = courseRepository;
            }


            public async Task<Result> Handle(RegisterCommand command)
            {
                var student = new Student(command.Name, command.Email);

                if (command.Course1 != null && command.Course1Grade != null)
                {
                    var courseResult = await _courseRepository.GetByName(command.Course1);
                    if (courseResult.HasNoValue)
                        return Result.Failure("Course not found");

                    student.Enroll(courseResult.Value, Enum.Parse<Grade>(command.Course1Grade));
                }

                if (command.Course2 != null && command.Course2Grade != null)
                {
                    var courseResult = await _courseRepository.GetByName(command.Course2);
                    if (courseResult.HasNoValue)
                        return Result.Failure("Course not found");

                    student.Enroll(courseResult.Value, Enum.Parse<Grade>(command.Course2Grade));
                }

                await _studentRepository.Update(student);
                await _studentRepository.Save();

                return Result.Success();
            }
        }
    }
}