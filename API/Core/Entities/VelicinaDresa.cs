using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class VelicinaDresa : BaseEntity
    {
        [Key]
        public Guid VelicinaDresaId { get; set; }

        [Required]
        [StringLength(5)]
        public string VelicinaDresaVrednost { get; set; }

        public int Kolicina { get; set; }
        public Dres Dres { get; set; }
        public Guid DresId { get; set; }

        [JsonIgnore]
        public StavkaPorudzbine StavkaPorudzbines { get; set; }
    }
}
