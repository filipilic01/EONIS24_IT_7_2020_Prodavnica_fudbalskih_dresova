using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Kupac
{
    public class KupacDto
    {
        public Guid KupacId { get; set; }

        public string KupacKorisnickoIme { get; set; }

        public string KupacIme { get; set; }

        public string KupacPrezime { get; set; }


        public string KupacEmail { get; set; }

        public string KupacBrojTelefona { get; set; }

        public string KupacAdresa { get; set; }
    }
}
