namespace API.Dtos.OrderItems
{
    public class OrderItemCreationDto
    {
        public int ItemNumber { get; set; }
        public Guid OrderId { get; set; }
        public Guid JerseySizeId { get; set; }
    }
}
