namespace API.Dtos.StavkaPorudzbine
{
    public class StavkaPorudzbineCreationDto
    {
        public int BrojStavki { get; set; }
        public Guid PorudzbinaId { get; set; }
        public Guid VelicinaDresaId { get; set; }
    }
}
