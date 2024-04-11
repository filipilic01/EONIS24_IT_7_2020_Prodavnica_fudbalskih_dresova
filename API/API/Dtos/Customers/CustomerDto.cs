using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Customers
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }

        public string CustomerUserName { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }


        public string CustomerEmail { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string CustomerAddress { get; set; }
    }
}
