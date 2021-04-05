using CSharpFunctionalExtensions;
using Extensions;
using Logic.Payments.Models;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Payments.Commands
{
    public sealed class PaySchoolFeeCommand : ICommand
    {
        public long StudentId { get; }
        public decimal Amount { get; }
        public string Currency { get; }

        public PaySchoolFeeCommand(long studentId, decimal amount, string currency)
        {
            StudentId = studentId;
            Amount = amount;
            Currency = currency;
        }
    }

    internal class PaySchoolFeeCommandHandler : ICommandHandler<PaySchoolFeeCommand>
    {
        private readonly IRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PaySchoolFeeCommandHandler(IRepository repository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        public Type CommandType => typeof(PaySchoolFeeCommand);

        public async Task<Result> Handle(PaySchoolFeeCommand command)
        {
            var student = await _repository.GetByIdAsync<Student>(command.StudentId);
            if (student == null)
                return Result.Failure($"Cannot find student with id: {command.StudentId}");

            var moneyResult = Money.Create(command.Amount, command.Currency);
            if (moneyResult.IsFailure)
                return Result.Failure(moneyResult.Error);

            var paymentResult = Payment.Create(moneyResult.Value, _dateTimeProvider.UtcNow, student);
            if(paymentResult.IsFailure)
                return Result.Failure(paymentResult.Error);

            await _repository.InsertAsync(paymentResult.Value);

            return Result.Success();
        }
    }
}
