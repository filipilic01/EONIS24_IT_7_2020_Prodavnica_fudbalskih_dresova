using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
   
    public class OrderItem : BaseEntity
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public int ItemNumber { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public JerseySize JerseySize { get; set; }
        public Guid JerseySizeId { get; set; }
    }
}
