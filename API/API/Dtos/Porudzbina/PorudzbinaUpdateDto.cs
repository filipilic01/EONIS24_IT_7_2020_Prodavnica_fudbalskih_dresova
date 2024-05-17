namespace API.Dtos.Porudzbina
{
    public class PorudzbinaUpdateDto
    {
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime DatumAzuriranja { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool Placena { get; set; }

    }
}
