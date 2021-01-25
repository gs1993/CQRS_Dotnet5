using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Repositories;
using Logic.Students;
using Logic.Utils;
using System.Threading.Tasks;

namespace Logic.Commands
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
            private readonly IGenericRepository<Student> _studentRepository;

            public DisenrollCommandHandler(IGenericRepository<Student> studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public async Task Handle(DisenrollCommand command)
            {
                Student student = _studentRepository.Get(command.Id);
                if (student == null)
                    return Result.Fail($"No student found for Id {command.Id}");

                if (string.IsNullOrWhiteSpace(command.Comment))
                    return Result.Fail("Disenrollment comment is required");

                Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollment == null)
                    return Result.Fail($"No enrollment found with number '{command.EnrollmentNumber}'");

                student.RemoveEnrollment(enrollment, command.Comment);

                unitOfWork.Commit();

                return Result.Ok();
            }
        }
    }
}