﻿namespace StellarStock.Domain.Entities.Base
{
    public class BaseEntity
    {
        [DataType(DataType.Text)]
        public string? Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}
