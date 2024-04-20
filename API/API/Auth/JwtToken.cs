namespace API.Auth
{
    public class JwtToken
    {

        public string Token { get; set; }
        public string ExpiresOn { get; set; }

        public string KorisnickoIme { get; set; }
        public string Role { get; set; }

        public Guid UserId { get; set; }
    }
}
