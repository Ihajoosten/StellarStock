namespace StellarStock.Domain.ValueObjects
{
    public class MoneyVO
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public MoneyVO(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}