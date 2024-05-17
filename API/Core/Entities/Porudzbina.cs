using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Porudzbina : BaseEntity
    {
        [Key]
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime? DatumAzuriranja { get; set; }
        public DateTime? DatumKreiranja { get; set; }
        public bool Placena { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public Kupac Kupac { get; set; }
        public Guid KupacId { get; set; }

        [JsonIgnore]
        public ICollection<StavkaPorudzbine> StavkaPorudzbines { get; set; }


    }
}
