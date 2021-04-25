using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Logic.Payments.Models
{
    public class Payment : Entity
    {
        public Money Amount { get; }
        public DateTime Date { get;  }
        public Student Student { get; }

        protected Payment() { }
        private Payment(Money amount, DateTime date, Student student)
        {
            Amount = amount;
            Date = date;
            Student = student;
        }

        public static Result<Payment> Create(Money amount, DateTime date, Student student)
        {
            if (amount == null)
                return Result.Failure<Payment>("Invalid amount");
            if (student == null)
                return Result.Failure<Payment>("Invalid student");

            return Result.Success(new Payment(amount, date, student));
        }

    }

    public class Student : Entity
    {

    }

    public class Money : ValueObject
    {
        public decimal Value { get; }
        public string Currency { get; }
        


        protected Money() { }
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
