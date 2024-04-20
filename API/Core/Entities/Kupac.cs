using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Kupac : BaseEntity
    {
        [Key]
        public Guid KupacId { get; set; }

        [Required]
        [StringLength(30)]
        public string KupacKorisnickoIme { get; set; }

        [Required]
        [StringLength(30)]
        public string KupacIme { get; set; }

        [Required]
        [StringLength(30)]
        public string KupacPrezime { get; set; }

        [Required]
        [StringLength(60)]
        public string KupacLozinka { get; set; }

        [Required]
        [StringLength(30)]
        public string KupacEmail { get; set; }

        [Required]
        [StringLength(30)]
        public string KupacBrojTelefona { get; set; }

        [Required]
        [StringLength(80)]
        public string KupacAdresa { get; set; }

        [JsonIgnore]
        public ICollection<Porudzbina> Porudzbinas { get; set; }
    }
}
