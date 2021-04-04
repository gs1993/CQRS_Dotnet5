using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Logic.Payments.Models
{
    public class Payment : Entity
    {
        public Student Student { get; set; }
        public Money Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Student : Entity
    {

    }

    public class Money : ValueObject
    {
        public decimal Value { get; }
        public string Currency { get; }


        public Money() { } //TODO: zmienić na protected

        private Money(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        public static Result<Money> Create(decimal value, string currency)
        {
            if (value <= 0)
                return Result.Failure<Money>("Invalid amount");

            if(string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                return Result.Failure<Money>("Invalid currency");

            return Result.Success(new Money(value, currency));
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}
