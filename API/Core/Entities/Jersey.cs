using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Jersey : BaseEntity
    {
        [Key]
        public Guid JerseyId { get; set; }

        [Required]
        [StringLength(40)]
        public string PlayerName { get; set; }

        [Required]
        [StringLength(30)]
        public string Team { get; set; }

        [Required]
        [StringLength(10)]
        public string Season { get; set; }

        [Required]
        [StringLength(20)]
        public string Brand { get; set; }
        public double Price { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; }

        [Required]
        [StringLength(30)]
        public string Country { get; set; }

        [Required]
        [StringLength(30)]
        public string Competition { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [Required]
        [StringLength(500)]
        public string TeamUrl { get; set; }
        public Admin Admin { get; set; }
        public Guid AdminId { get; set; }

        [JsonIgnore]
        public ICollection<JerseySize> JerseySizes { get; set; }


    }
}
