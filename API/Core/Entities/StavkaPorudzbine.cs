using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
   
    public class StavkaPorudzbine : BaseEntity
    {
        [Key]
        public Guid StavkaPorudzbineId { get; set; }
        public int BrojStavki { get; set; }
        public Porudzbina Porudzbina { get; set; }
        public Guid PorudzbinaId { get; set; }
        public VelicinaDresa VelicinaDresa { get; set; }
        public Guid VelicinaDresaId { get; set; }
    }
}
