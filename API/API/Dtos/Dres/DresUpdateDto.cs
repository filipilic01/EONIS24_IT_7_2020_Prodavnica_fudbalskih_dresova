namespace API.Dtos.Dres
{
    public class DresUpdateDto
    {
        public Guid DresId { get; set; }
        public string ImeIgraca { get; set; }
        public string Tim { get; set; }
        public string Sezona { get; set; }
        public string Brend { get; set; }
        public double Cena { get; set; }
        public string SlikaUrl { get; set; }
        public string Tip { get; set; }
        public string Zemlja { get; set; }
        public string Takmicenje { get; set; }
        public string Status { get; set; }
        public string TimUrl { get; set; }
    }
}
