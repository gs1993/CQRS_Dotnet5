using CSharpFunctionalExtensions;
using Logic.Payments.Models;
using Logic.Students.Repositories;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;

namespace Logic.Payments.Commands
{
    public sealed class PaySchoolFeeCommand : ICommand
    {
        public long StudentId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
    }

    internal class PaySchoolFeeCommandHandler : ICommandHandler<PaySchoolFeeCommand>
    {
        private readonly IGenericRepository<Payment> _paymentRepository;

        public Type CommandType => typeof(PaySchoolFeeCommand);

        public Task<Result> Handle(PaySchoolFeeCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
