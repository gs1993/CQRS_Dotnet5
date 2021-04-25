using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Studentss.Commands
{
    public sealed class DisenrollCommand : ICommand
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }

        public DisenrollCommand(long id, int enrollmentNumber, string comment)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Comment = comment;
        }


        internal sealed class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
        {
            private readonly IRepository _repository;

            public DisenrollCommandHandler(IRepository repository)
            {
                _repository = repository;
            }

            public Type CommandType => typeof(DisenrollCommand);

            public async Task<Result> Handle(DisenrollCommand command)
            {
                var student = await _repository.GetByIdAsync<Student>(command.Id);
                if (student == null)
                    return Result.Failure($"No student found for Id {command.Id}");

                if (string.IsNullOrWhiteSpace(command.Comment))
                    return Result.Failure("Disenrollment comment is required");

                var enrollmentResult = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollmentResult.HasNoValue)
                    return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

                student.RemoveEnrollment(enrollmentResult.Value, command.Comment);

                await _repository.UpdateAsync(student);

                return Result.Success();
            }
        }
    }
}