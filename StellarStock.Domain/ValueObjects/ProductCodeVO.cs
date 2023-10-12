using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.ValueObjects
{
    public class ProductCodeVO
    {
        [Required(ErrorMessage = "Code is required.")]
        [MaxLength(25, ErrorMessage = "Code cannot exceed 25 characters.")]
        public string Code { get; }

        public ProductCodeVO(string code)
        {
            Code = code;
        }
    }
}
