using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderWithCustomerSpecification : BaseSpecification<Order>
    {
        public OrderWithCustomerSpecification()
        {
            AddInclude(x => x.Customer);
        }

        public OrderWithCustomerSpecification(Guid id) : base(x => x.OrderId == id)
        {
            AddInclude(x => x.Customer);
        }
    }
}
