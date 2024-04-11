namespace API.Dtos.Orders
{
    public class OrderCreationDto
    {
        public double OrderTotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
    }
}
