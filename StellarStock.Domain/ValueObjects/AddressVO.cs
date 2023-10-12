using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.ValueObjects
{
    public class AddressVO
    {
        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(50, ErrorMessage = "Street cannot exceed 50 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(25, ErrorMessage = "City cannot exceed 25 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(25, ErrorMessage = "City cannot exceed 25 characters.")]
        public string Region { get; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Postal Code must be between 5 and 10 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        public AddressVO(string street, string postalCode, string city, string region, string country)
        {
            Street = street;
            PostalCode = postalCode;
            City = city;
            Region = region;
            Country = country;
        }
    }
}
