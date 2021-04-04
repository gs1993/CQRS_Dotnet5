using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Shared;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Students.Commands
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
            private readonly IRepository _repository;

            public EnrollCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(EnrollCommand);

            public async Task<Result> Handle(EnrollCommand command)
            {
                var student = await _repository.GetByIdAsync<Student>(command.Id);
                if (student == null)
                    return Result.Failure($"No student found with Id '{command.Id}'");

                var course = await _repository.GetAsync<Course>(c => c.Name == command.Course);
                if (course == null)
                    return Result.Failure($"Course is incorrect: '{command.Course}'");

                bool success = Enum.TryParse(command.Grade, out Grade grade);
                if (!success)
                    return Result.Failure($"Grade is incorrect: '{command.Grade}'");

                student.Enroll(course, grade);

                await _repository.UpdateAsync(student);

                return Result.Success();
            }
        }
    }
}