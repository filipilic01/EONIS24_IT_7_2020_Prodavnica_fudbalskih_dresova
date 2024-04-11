using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer : BaseEntity
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerUserName { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerFirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerLastName { get; set; }

        [Required]
        [StringLength(60)]
        public string CustomerPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerEmail { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerPhoneNumber { get; set; }

        [Required]
        [StringLength(80)]
        public string CustomerAddress { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
