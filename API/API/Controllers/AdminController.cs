using API.Dtos.Admins;
using API.Dtos.Jerseys;
using API.Dtos.JerseySizes;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<AdminDto>> AddAdmin([FromBody] AdminCreationDto adminPost)
        {
            try
            {
                var existingAdmin = _repository.GetUsername(adminPost.AdminUserName, "Admin");
                if (existingAdmin.Equals(true))
                    return BadRequest(new ApiResponse(400, "Username " + adminPost.AdminUserName+ " already exixsts!"));
                Admin adminEntity = _mapper.Map<Admin>(adminPost);
                adminEntity.AdminId = Guid.NewGuid();
                adminEntity.AdminPassword = BCrypt.Net.BCrypt.HashPassword(adminPost.AdminPassword);

                await _repository.AddAsync(adminEntity);
               
               
                return Ok(_mapper.Map<Admin, AdminDto>(adminEntity));
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
        public async Task<ActionResult<AdminDto>> UpdateAdmin([FromBody] AdminUpdateDto adminUpdate)
        {
            try
            {
                var adminEntity = await _repository.GetByIdAsync(adminUpdate.AdminId);

                if (adminEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Admin with " + adminUpdate.AdminId + " not found"));
                }

                Admin admin = _mapper.Map<Admin>(adminUpdate);

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
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }
    }
}
