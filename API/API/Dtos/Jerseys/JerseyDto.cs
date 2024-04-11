namespace API.Dtos.Jerseys
{
    public class JerseyDto
    {
        public Guid JerseyId { get; set; }
        public string PlayerName { get; set; }
        public string Team { get; set; }
        public string Season { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string Competition { get; set; }
        public string Status { get; set; }
        public string TeamUrl { get; set; }
        public string Admin { get; set; }
    }
}
