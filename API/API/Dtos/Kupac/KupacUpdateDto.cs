namespace API.Dtos.Kupac
{
    public class KupacUpdateDto
    {
        public Guid KupacId { get; set; }

        public string KupacIme { get; set; }

        public string KupacPrezime { get; set; }

 

        public string KupacBrojTelefona { get; set; }

        public string KupacAdresa { get; set; }
    }
}
