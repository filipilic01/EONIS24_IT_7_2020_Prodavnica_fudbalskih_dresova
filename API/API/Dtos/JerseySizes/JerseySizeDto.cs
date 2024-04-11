using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.JerseySizes
{
    public class JerseySizeDto
    {
        public Guid JerseySizeId { get; set; }
        public string JerseySizeValue { get; set; }
        public int Quantity { get; set; }
        public string Jersey { get; set; }
  
    }
}
