using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Dres : BaseEntity
    {
        [Key]
        public Guid DresId { get; set; }

      
        [StringLength(40)]
        public string ImeIgraca { get; set; }

        [Required]
        [StringLength(30)]
        public string Tim { get; set; }

        [Required]
        [StringLength(10)]
        public string Sezona { get; set; }

        [Required]
        [StringLength(20)]
        public string Brend { get; set; }
        public double Cena { get; set; }

        [Required]
        
        public string SlikaUrl { get; set; }

        
        public bool Obrisan { get; set; }

        [Required]
        [StringLength(10)]
        public string Tip { get; set; }

        [Required]
        [StringLength(30)]
        public string Zemlja { get; set; }

        [Required]
        [StringLength(30)]
        public string Takmicenje { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [Required]
        [StringLength(500)]
        public string TimUrl { get; set; }
        public Admin Admin { get; set; }
        public Guid AdminId { get; set; }

        [JsonIgnore]
        public ICollection<VelicinaDresa> VelicinaDresas { get; set; }


    }
}
