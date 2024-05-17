using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PorudzbinaPaymentDto
    {
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime DatumAzuriranja { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool Placena { get; set; }
        public string Kupac { get; set; }

        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }

    }
}
