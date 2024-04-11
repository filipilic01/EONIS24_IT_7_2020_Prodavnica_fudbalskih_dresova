using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.OrderItems
{
    public class OrderItemDto
    {
        public Guid OrderItemId { get; set; }
        public int ItemNumber { get; set; }
        public string Order { get; set; }
        public string JerseySize { get; set; }
       
    }
}
