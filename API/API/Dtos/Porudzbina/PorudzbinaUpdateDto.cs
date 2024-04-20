namespace API.Dtos.Porudzbina
{
    public class PorudzbinaUpdateDto
    {
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime DatumPorudzbine { get; set; }

    }
}
