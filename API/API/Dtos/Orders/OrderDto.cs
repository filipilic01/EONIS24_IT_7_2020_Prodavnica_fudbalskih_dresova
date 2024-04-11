using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.Orders
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public double OrderTotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; }
 
    }
}
