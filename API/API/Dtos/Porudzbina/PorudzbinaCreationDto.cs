namespace API.Dtos.Porudzbina
{
    public class PorudzbinaCreationDto
    {
        public double UkupanIznos { get; set; }
        public DateTime DatumPorudzbine { get; set; }
        public Guid KupacId { get; set; }
    }
}
