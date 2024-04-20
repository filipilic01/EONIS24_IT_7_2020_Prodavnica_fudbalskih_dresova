namespace API.Dtos.VelicinaDresa
{
    public class VelicinaDresaUpdateDto
    {
        public Guid VelicinaDresaId { get; set; }
        public string VelicinaDresaVrednost { get; set; }
        public int Kolicina { get; set; }
    }
}
