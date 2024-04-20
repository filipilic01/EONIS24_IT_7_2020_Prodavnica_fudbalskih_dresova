using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.VelicinaDresa
{
    public class VelicinaDresaDto
    {
        public Guid VelicinaDresaId { get; set; }
        public string VelicinaDresaVrednost { get; set; }
        public int Kolicina { get; set; }
        public string Dres { get; set; }
  
    }
}
