using API.Dtos.Admins;
using API.Dtos.Jerseys;
using API.Dtos.JerseySizes;
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
        [Authorize(Roles = "Admin")]
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
                return NotFound(new ApiResponse(404, "Admin with ID " + adminId + " not found"));

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
                    var existingAdminUsername = _repository.GetUsername(adminPost.AdminUserName, "Admin");
                    if (existingAdminUsername.Equals(true))
                        return BadRequest(new ApiResponse(400, "Username " + adminPost.AdminUserName + " already exixsts!"));
                    var existingAdminEmail = _repository.GetEmail(adminPost.AdminEmail, "Admin");
                    if (existingAdminEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + adminPost.AdminEmail + " already exixsts!"));
                    Admin adminEntity = _mapper.Map<Admin>(adminPost);
                    adminEntity.AdminId = Guid.NewGuid();
                    adminEntity.AdminPassword = BCrypt.Net.BCrypt.HashPassword(adminPost.AdminPassword);

                    await _repository.AddAsync(adminEntity);


                    return Ok(_mapper.Map<Admin, AdminDto>(adminEntity));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Incorrect email format"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Creating error"));
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

                    return NotFound(new ApiResponse(404, "Admin with ID " + adminId + " not found"));
                }

                await _repository.DeleteAsync(adminId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Deleting error"));
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

                        return NotFound(new ApiResponse(404, "Admin with " + adminUpdate.AdminId + " not found"));
                    }

                    Admin admin = _mapper.Map<Admin>(adminUpdate);
                    var existingAdminUsername = _repository.GetUsername(admin.AdminUserName, "Admin");
                    if (existingAdminUsername.Equals(true))
                        return BadRequest(new ApiResponse(400, "Username " + admin.AdminUserName + " already exixsts!"));
                    var existingAdminEmail = _repository.GetEmail(admin.AdminEmail, "Admin");
                    if (existingAdminEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + admin.AdminEmail + " already exixsts!"));
                    var updateJersey = await _repository.UpdateAsync(admin, adminEntity, (existingAdmin, newAdmin) =>
                    {
                        existingAdmin.AdminId = newAdmin.AdminId;
                        existingAdmin.AdminFirstName = newAdmin.AdminFirstName;
                        existingAdmin.AdminLastName = newAdmin.AdminLastName;
                        existingAdmin.AdminUserName = newAdmin.AdminUserName;
                        existingAdmin.AdminPassword = newAdmin.AdminPassword;
                        existingAdmin.AdminPhoneNumber = newAdmin.AdminPhoneNumber;
                        existingAdmin.AdminEmail = newAdmin.AdminEmail;
                        existingAdmin.AdminAddress = newAdmin.AdminAddress;

                        return existingAdmin;
                    });

                    var admin_2 = await _repository.GetByIdAsync(adminUpdate.AdminId);
                    return Ok(_mapper.Map<Admin, AdminDto>(admin_2));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Incorrect email format"));
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
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
