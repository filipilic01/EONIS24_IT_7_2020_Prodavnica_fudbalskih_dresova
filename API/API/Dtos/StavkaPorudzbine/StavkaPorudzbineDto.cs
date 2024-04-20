using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.StavkaPorudzbine
{
    public class StavkaPorudzbineDto
    {
        public Guid StavkaPorudzbineId { get; set; }
        public int BrojStavki { get; set; }
        public string Porudzbina { get; set; }
        public string VelicinaDresa { get; set; }
       
    }
}
