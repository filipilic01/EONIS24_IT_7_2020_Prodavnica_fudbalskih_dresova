using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderItemWithOrderAndJerseySizeSpecification:  BaseSpecification<OrderItem>
    {
        public OrderItemWithOrderAndJerseySizeSpecification()
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.JerseySize);
        }

        public OrderItemWithOrderAndJerseySizeSpecification(Guid id) : base(x => x.OrderItemId == id)
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.JerseySize);
        }
    }
}
