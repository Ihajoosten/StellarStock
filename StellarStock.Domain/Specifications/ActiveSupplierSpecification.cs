using StellarStock.Domain.Entities;
using StellarStock.Domain.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarStock.Domain.Specifications
{
    public class ActiveSupplierSpecification : ISpecification<Supplier>
    {
        public bool IsSatisfiedBy(Supplier supplier)
        {
            return supplier.IsActive && supplier.ValidityPeriod.IsSatisfiedBy(DateTime.Now);
        }
    }

}
