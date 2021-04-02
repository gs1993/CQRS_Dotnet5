using System.Security.Cryptography;

namespace ApiClient.MastercardConversionRate.Config
{
    public record SigningKey
    {
        public RSA Key { get; init; }
    }
}
