namespace StellarStock.Domain.ValueObjects
{
    public class DateRangeVO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRangeVO(DateTime startDate, DateTime endDate)
        {
            // Validation logic (e.g., end date should be greater than or equal to start date)
            if (endDate < startDate)
            {
                throw new ArgumentException("End date should be greater than or equal to start date.");
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsSatisfiedBy(DateTime dateToCheck)
        {
            return dateToCheck >= StartDate && dateToCheck <= EndDate;
        }
    }
}
