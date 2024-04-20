namespace API.Dtos.VelicinaDresa
{
    public class VelicinaDresaCreationDto
    {
        public string VelicinaDresaVrednost { get; set; }
        public int Kolicina { get; set; }
        public Guid DresId { get; set; }
    }
}
