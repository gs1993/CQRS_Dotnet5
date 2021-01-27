using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Repositories;

namespace Logic.Commands
{
    public sealed class EnrollCommand : ICommand
    {
        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }

        public EnrollCommand(long id, string course, string grade)
        {
            Id = id;
            Course = course;
            Grade = grade;
        }

        internal sealed class EnrollCommandHandler : ICommandHandler<EnrollCommand>
        {
            private readonly IGenericRepository<Student> _studentRepository;
            private readonly ICourseRepository _courseRepository;

            public EnrollCommandHandler(IGenericRepository<Student> studentRepository, ICourseRepository courseRepository)
            {
                _studentRepository = studentRepository;
                _courseRepository = courseRepository;
            }


            public async Task<Result> Handle(EnrollCommand command)
            {
                var studentResult = await _studentRepository.Get(command.Id);
                if (studentResult.HasNoValue)
                    return Result.Failure($"No student found with Id '{command.Id}'");

                var courseResult = await _courseRepository.GetByName(command.Course);
                if (courseResult.HasNoValue)
                    return Result.Failure($"Course is incorrect: '{command.Course}'");

                bool success = Enum.TryParse(command.Grade, out Grade grade);
                if (!success)
                    return Result.Failure($"Grade is incorrect: '{command.Grade}'");

                var student = studentResult.Value;
                var course = courseResult.Value;
                student.Enroll(course, grade);

                await _studentRepository.Save();

                return Result.Success();
            }
        }
    }
}