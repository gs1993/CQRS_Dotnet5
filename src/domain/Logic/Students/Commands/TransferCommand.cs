using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Students.Repositories;
using Logic.Utils.Shared;

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
            private readonly IGenericRepository<Student> _studentRepository;
            private readonly ICourseRepository _courseRepository;

            public TransferCommandHandler(IGenericRepository<Student> studentRepository, ICourseRepository courseRepository)
            {
                _studentRepository = studentRepository;
                _courseRepository = courseRepository;
            }

            public Type CommandType => typeof(TransferCommand);

            public async Task<Result> Handle(TransferCommand command)
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
                var enrollmentResult = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollmentResult.HasNoValue)
                    return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

                enrollmentResult.Value.Update(course, grade);

                await _studentRepository.Save();

                return Result.Success();
            }
        }
    }
}