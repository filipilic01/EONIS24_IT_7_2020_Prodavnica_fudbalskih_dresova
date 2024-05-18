using API.Auth;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IGenericRepository<Admin> _adminRepository;
        private readonly IGenericRepository<Kupac> _kupacRepository;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AuthController(IGenericRepository<Admin> adminRepository, IGenericRepository<Kupac> kupacRepository, IJwtAuthManager jwtAuthManager)
        {
            _adminRepository = adminRepository;
            _kupacRepository = kupacRepository;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthCreds authCreds)
        {
            Admin admin = _adminRepository.GetAdminByKorisnickoIme(authCreds.KorisnickoIme);

            Kupac kupac = _kupacRepository.GetKupacByKorisnickoIme(authCreds.KorisnickoIme);

            if (admin == null && kupac == null)
            {
                return BadRequest(new ApiResponse(400, "Korisničko ime nije ispravno"));
            }

            if (admin != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(authCreds.Lozinka, admin.AdminLozinka))
                {
                    return Unauthorized(new ApiResponse(401, "Neispravna lozinka"));
                }
                else
                {
                    var token = _jwtAuthManager.Authenticate(authCreds.KorisnickoIme, authCreds.Lozinka, "Admin", admin.AdminId);
                    return Ok(new { Token = token.Token, ExpiresOn = token.ExpiresOn, KorisnickoIme = token.KorisnickoIme,Role = token.Role, UserId = token.UserId });
                }


            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(authCreds.Lozinka, kupac.KupacLozinka))
                {
                    return Unauthorized(new ApiResponse(401, "Pogrešna lozinka"));
                }
                else
                {
                    var token = _jwtAuthManager.Authenticate(authCreds.KorisnickoIme, authCreds.Lozinka,"Kupac", kupac.KupacId);
                    return Ok(new { Token = token.Token, ExpiresOn = token.ExpiresOn, KorisnickoIme = token.KorisnickoIme,Role = token.Role, UserId = token.UserId });
                }
            }



        }

        [Authorize(Roles = "Admin, Kupac")]
        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst("UserId");
            var userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
            var jwtToken = HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            var expiresOn = DateTime.UtcNow.AddHours(5);


            var currentUser = new
            {
                Token = jwtToken,
                ExpiresOn = expiresOn,
                KorisnickoIme = userName,
                Role = userRole,
                UserId = userId
                
            };

            return Ok(currentUser);
        }


    }
}
