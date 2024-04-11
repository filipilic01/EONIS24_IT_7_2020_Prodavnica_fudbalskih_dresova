namespace API.Dtos.Orders
{
    public class OrderUpdateDto
    {
        public Guid OrderId { get; set; }
        public double OrderTotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
