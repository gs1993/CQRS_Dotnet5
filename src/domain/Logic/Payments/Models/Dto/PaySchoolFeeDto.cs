namespace Logic.Payments.Models.Dto
{
    public sealed class PaySchoolFeeDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public long StudentId { get; set; }
    }
}
