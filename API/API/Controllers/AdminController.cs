using API.Dtos.Admin;

using API.Dtos.Dres;
using API.Dtos.VelicinaDresa;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IGenericRepository<Admin> _repository;
        private readonly IMapper _mapper;

        public AdminController(IGenericRepository<Admin> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<List<AdminDto>>> GetAdmins()
        {
         
            var admins = await _repository.GetAllAsync();

            if (admins == null || admins.Count == 0)
            {
                return NoContent();
            }

            var adminsDto = _mapper.Map<IEnumerable<Admin>, IEnumerable<AdminDto>>(admins);
            return Ok(adminsDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{adminId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminDto>> GetAdminById(Guid adminId)
        {
           
            var admin = await _repository.GetByIdAsync(adminId);

            if (admin == null)
                return NotFound(new ApiResponse(404, "Admin sa ID " + adminId + " nije pronadjen"));

            return _mapper.Map<Admin, AdminDto>(admin);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]

        public async Task<ActionResult<AdminDto>> AddAdmin([FromBody] AdminCreationDto adminPost)
        {
            try
            {
                if (IsValidEmail(adminPost.AdminEmail))
                {
                    var existingAdminKorisnickoIme = _repository.GetKorisnickoIme(adminPost.AdminKorisnickoIme, "Admin");
                    if (existingAdminKorisnickoIme.Equals(true))
                        return BadRequest(new ApiResponse(400, "KorisnickoIme " + adminPost.AdminKorisnickoIme + " vec postoji!"));
                    var existingAdminEmail = _repository.GetEmail(adminPost.AdminEmail, "Admin");
                    if (existingAdminEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + adminPost.AdminEmail + " vec postoji!"));
                    Admin adminEntity = _mapper.Map<Admin>(adminPost);
                    adminEntity.AdminId = Guid.NewGuid();
                    adminEntity.AdminLozinka = BCrypt.Net.BCrypt.HashPassword(adminPost.AdminLozinka);

                    await _repository.AddAsync(adminEntity);


                    return Ok(_mapper.Map<Admin, AdminDto>(adminEntity));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Nepravilan format mejla"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja admina"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{adminId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdmin(Guid adminId)
        {
            try
            {
                var admin = await _repository.GetByIdAsync(adminId);

                if (admin == null)
                {

                    return NotFound(new ApiResponse(404, "Admin sa ID " + adminId + "nije pronadjen"));
                }

                await _repository.DeleteAsync(adminId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja admina."));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminDto>> UpdateAdmin([FromBody] AdminUpdateDto adminUpdate)
        {
            try
            {
                if (IsValidEmail(adminUpdate.AdminEmail))
                {
                    var adminEntity = await _repository.GetByIdAsync(adminUpdate.AdminId);

                    if (adminEntity == null)
                    {

                        return NotFound(new ApiResponse(404, "Admin sa " + adminUpdate.AdminId + " nije pronadjen"));
                    }

                    Admin admin = _mapper.Map<Admin>(adminUpdate);
                    var existingAdminKorisnickoIme = _repository.GetKorisnickoIme(admin.AdminKorisnickoIme, "Admin");
                    if (existingAdminKorisnickoIme.Equals(true))
                        return BadRequest(new ApiResponse(400, "KorisnickoIme " + admin.AdminKorisnickoIme + " vec postoji!"));
                    var existingAdminEmail = _repository.GetEmail(admin.AdminEmail, "Admin");
                    if (existingAdminEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + admin.AdminEmail + "vec postoji!"));
                    var updateDres = await _repository.UpdateAsync(admin, adminEntity, (existingAdmin, newAdmin) =>
                    {
                        existingAdmin.AdminId = newAdmin.AdminId;
                        existingAdmin.AdminIme = newAdmin.AdminIme;
                        existingAdmin.AdminPrezime = newAdmin.AdminPrezime;
                        existingAdmin.AdminKorisnickoIme = newAdmin.AdminKorisnickoIme;
                        existingAdmin.AdminLozinka = BCrypt.Net.BCrypt.HashPassword(newAdmin.AdminLozinka);
                        existingAdmin.AdminBrojTelefona = newAdmin.AdminBrojTelefona;
                        existingAdmin.AdminEmail = newAdmin.AdminEmail;
                        existingAdmin.AdminAdresa = newAdmin.AdminAdresa;

                        return existingAdmin;
                    });

                    var admin_2 = await _repository.GetByIdAsync(adminUpdate.AdminId);
                    return Ok(_mapper.Map<Admin, AdminDto>(admin_2));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Nepravilan format mejla"));
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja admina"));
            }

        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

    }
}
