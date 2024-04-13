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
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AuthController(IGenericRepository<Admin> adminRepository, IGenericRepository<Customer> customerRepository, IJwtAuthManager jwtAuthManager)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthCreds authCreds)
        {
            Admin admin = _adminRepository.GetAdminByUsername(authCreds.UserName);

            Customer customer = _customerRepository.GetCustomerByUsername(authCreds.UserName);

            if (admin == null && customer == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            if (admin != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(authCreds.Password, admin.AdminPassword))
                {
                    return Unauthorized(new ApiResponse(401, "Invalid password"));
                }
                else
                {
                    var token = _jwtAuthManager.Authenticate(authCreds.UserName, authCreds.Password, "Admin", admin.AdminId);
                    return Ok(new { Token = token.Token, ExpiresOn = token.ExpiresOn, Username = token.Username,Role = token.Role, UserId = token.UserId });
                }


            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(authCreds.Password, customer.CustomerPassword))
                {
                    return Unauthorized(new ApiResponse(401, "Invalid password"));
                }
                else
                {
                    var token = _jwtAuthManager.Authenticate(authCreds.UserName, authCreds.Password,"Customer", customer.CustomerId);
                    return Ok(new { Token = token.Token, ExpiresOn = token.ExpiresOn, Username = token.Username,Role = token.Role, UserId = token.UserId });
                }
            }



        }


    }
}
