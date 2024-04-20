using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Auth
{
    public class JwtAuthManager: IJwtAuthManager
    {
        private readonly string _key;
        private readonly IConfiguration configuration;

        public JwtAuthManager(string key, IConfiguration configuration)
        {
            this._key = key;
            this.configuration = configuration;
        }

        public JwtToken Authenticate(string KorisnickoIme, string Lozinka, string role, Guid UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = configuration["ApplicationSettings:JWT_Secret"].ToString(); // Retrieve the JWT secret key
            var tokenKey = Encoding.ASCII.GetBytes(secret); // Convert the secret key to a byte array
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, KorisnickoIme),
                    new Claim("JWTID", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("UserId", UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            JwtToken tokenRes = new JwtToken
            {
                Token = tokenHandler.WriteToken(token),

                ExpiresOn = String.Format("{0:dd-MM-yyyy hh:mm:ss}", (DateTime)tokenDescriptor.Expires),

                KorisnickoIme = KorisnickoIme,
                Role = role,
                UserId = UserId


            };
            // Log token details in Authenticate method
            // Console.WriteLine($"Generated Token: {tokenHandler.WriteToken(token)}");
            return tokenRes;
        }
    }
}
