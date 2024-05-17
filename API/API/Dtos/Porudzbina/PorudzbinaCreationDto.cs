namespace API.Dtos.Porudzbina
{
    public class PorudzbinaCreationDto
    {
        public double UkupanIznos { get; set; }
        public DateTime DatumAzuriranja { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool Placena { get; set; }
        public Guid KupacId { get; set; }
    }
}
