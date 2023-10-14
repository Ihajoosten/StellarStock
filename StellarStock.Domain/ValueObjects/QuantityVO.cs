namespace StellarStock.Domain.ValueObjects
{
    public class QuantityVO
    {
        public int Value { get; set; }

        public QuantityVO(int value)
        {
            if (value < 0)
            {
                // Handle invalid quantity (depends on your business rules)
                throw new ArgumentException("Quantity cannot be negative.");
            }

            Value = value;
        }
    }
}
