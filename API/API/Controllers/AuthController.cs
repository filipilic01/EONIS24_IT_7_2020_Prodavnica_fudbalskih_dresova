using API.Auth;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound(new ApiResponse(404, "User nije pronadjen"));
            }

            if (admin != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(authCreds.Lozinka, admin.AdminLozinka))
                {
                    return Unauthorized(new ApiResponse(401, "Invalid Lozinka"));
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
                    return Unauthorized(new ApiResponse(401, "Invalid Lozinka"));
                }
                else
                {
                    var token = _jwtAuthManager.Authenticate(authCreds.KorisnickoIme, authCreds.Lozinka,"Kupac", kupac.KupacId);
                    return Ok(new { Token = token.Token, ExpiresOn = token.ExpiresOn, KorisnickoIme = token.KorisnickoIme,Role = token.Role, UserId = token.UserId });
                }
            }



        }


    }
}
