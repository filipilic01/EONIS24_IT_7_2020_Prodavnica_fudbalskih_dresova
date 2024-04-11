using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class JerseySize : BaseEntity
    {
        [Key]
        public Guid JerseySizeId { get; set; }

        [Required]
        [StringLength(5)]
        public string JerseySizeValue { get; set; }

        public int Quantity { get; set; }
        public Jersey Jersey { get; set; }
        public Guid JerseyId { get; set; }

        [JsonIgnore]
        public OrderItem OrderItem { get; set; }
    }
}
