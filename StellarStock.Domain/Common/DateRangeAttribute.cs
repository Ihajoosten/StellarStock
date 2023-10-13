using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateTime MinDate { get; }
        public DateTime MaxDate { get; }

        public DateRangeAttribute(string minDate)
        {
            MinDate = DateTime.Parse(minDate);
            MaxDate = DateTime.Now;  // Set the end date to the current date
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < MinDate || date > MaxDate)
                {
                    return new ValidationResult($"The {validationContext.DisplayName} must be between {MinDate.ToShortDateString()} and {MaxDate.ToShortDateString()}.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
