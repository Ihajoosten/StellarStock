using StellarStock.Domain.Entities.Base;

namespace StellarStock.Domain.ValueObjects
{
    public class MoneyVO
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public MoneyVO(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}