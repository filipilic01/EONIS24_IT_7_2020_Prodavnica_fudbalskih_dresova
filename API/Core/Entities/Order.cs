using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        [Key]
        public Guid OrderId { get; set; }
        public double OrderTotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }


    }
}
