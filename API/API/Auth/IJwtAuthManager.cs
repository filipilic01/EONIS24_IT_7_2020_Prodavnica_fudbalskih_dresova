namespace API.Auth
{
    public interface IJwtAuthManager
    {
        JwtToken Authenticate(string KorisnickoIme, string Lozinka, string Role, Guid UserId);
    }
}
