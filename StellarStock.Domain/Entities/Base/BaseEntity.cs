using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities.Base
{
    public class BaseEntity
    {
        [DataType(DataType.Text)]
        public required string Id { get; set; } = new Guid().ToString();

        [DataType(DataType.DateTime)]
        public required DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime UpdatedAt { get; set; }
    }
}
