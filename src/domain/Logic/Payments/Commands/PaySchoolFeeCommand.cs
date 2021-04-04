using CSharpFunctionalExtensions;
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


        public Type CommandType => typeof(PaySchoolFeeCommand);

        public Task<Result> Handle(PaySchoolFeeCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
