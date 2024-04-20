using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.Porudzbina
{
    public class PorudzbinaDto
    {
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime DatumPorudzbine { get; set; }
        public string Kupac { get; set; }
 
    }
}
