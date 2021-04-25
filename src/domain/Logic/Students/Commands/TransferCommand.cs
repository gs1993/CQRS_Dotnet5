using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Shared;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Students.Commands
{
    public sealed class TransferCommand : ICommand
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Course { get; }
        public string Grade { get; }

        public TransferCommand(long id, int enrollmentNumber, string course, string grade)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Course = course;
            Grade = grade;
        }


        internal sealed class TransferCommandHandler : ICommandHandler<TransferCommand>
        {
            private readonly IRepository _repository;

            public TransferCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(TransferCommand);

            public async Task<Result> Handle(TransferCommand command)
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

                var enrollmentResult = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollmentResult.HasNoValue)
                    return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

                enrollmentResult.Value.Update(course, grade);

                await _repository.UpdateAsync(enrollmentResult.Value);

                return Result.Success();
            }
        }
    }
}