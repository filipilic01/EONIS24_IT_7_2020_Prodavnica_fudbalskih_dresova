using API.Dtos.Admins;
using API.Dtos.Customers;
using API.Dtos.OrderItems;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IGenericRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerController(IGenericRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        {

            var customers = await _repository.GetAllAsync();

            if (customers == null || customers.Count == 0)
            {
                return NoContent();
            }

            var customersDto = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers);
            return Ok(customersDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid customerId)
        {

            var customer = await _repository.GetByIdAsync(customerId);

            if (customer == null)
                return NotFound(new ApiResponse(404, "Customer with ID " + customerId + " not found"));

            return _mapper.Map<Customer, CustomerDto>(customer);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerCreationDto customerPost)
        {
            try
            {
                if (IsValidEmail(customerPost.CustomerEmail))
                {
                    var existingCustomerUsername = _repository.GetUsername(customerPost.CustomerUserName, "Customer");
                    if (existingCustomerUsername.Equals(true))
                        return BadRequest(new ApiResponse(400, "Username " + customerPost.CustomerUserName + " already exixsts!"));
                    var existingCustomerEmail = _repository.GetEmail(customerPost.CustomerEmail, "Customer");
                    if (existingCustomerEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + customerPost.CustomerEmail + " already exixsts!"));
                    Customer customerEntity = _mapper.Map<Customer>(customerPost);
                    customerEntity.CustomerId = Guid.NewGuid();
                    customerEntity.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customerPost.CustomerPassword);

                    await _repository.AddAsync(customerEntity);


                    return Ok(_mapper.Map<Customer, CustomerDto>(customerEntity));
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
        [HttpDelete("{customerId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(customerId);

                if (customer == null)
                {

                    return NotFound(new ApiResponse(404, "Customer with ID " + customerId + " not found"));
                }

                await _repository.DeleteAsync(customerId);


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
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer([FromBody] CustomerUpdateDto customerUpdate)
        {
            try
            {
                if (IsValidEmail(customerUpdate.CustomerEmail))
                {
                    var customerEntity = await _repository.GetByIdAsync(customerUpdate.CustomerId);

                    if (customerEntity == null)
                    {

                        return NotFound(new ApiResponse(404, "Customer with " + customerUpdate.CustomerId + " not found"));
                    }

                    Customer customer = _mapper.Map<Customer>(customerUpdate);
                    var existingCustomerUsername = _repository.GetUsername(customer.CustomerUserName, "Customer");
                    if (existingCustomerUsername.Equals(true))
                        return BadRequest(new ApiResponse(400, "Username " + customer.CustomerUserName + " already exixsts!"));
                    var existingCustomerEmail = _repository.GetEmail(customer.CustomerEmail, "Customer");
                    if (existingCustomerEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + customer.CustomerEmail + " already exixsts!"));
                    var updateJersey = await _repository.UpdateAsync(customer, customerEntity, (existingCustomer, newCustomer) =>
                    {
                        existingCustomer.CustomerId = newCustomer.CustomerId;
                        existingCustomer.CustomerFirstName = newCustomer.CustomerFirstName;
                        existingCustomer.CustomerLastName = newCustomer.CustomerLastName;
                        existingCustomer.CustomerUserName = newCustomer.CustomerUserName;
                        existingCustomer.CustomerPassword = newCustomer.CustomerPassword;
                        existingCustomer.CustomerPhoneNumber = newCustomer.CustomerPhoneNumber;
                        existingCustomer.CustomerEmail = newCustomer.CustomerEmail;
                        existingCustomer.CustomerAddress = newCustomer.CustomerAddress;

                        return existingCustomer;
                    });

                    var customer_2 = await _repository.GetByIdAsync(customerUpdate.CustomerId);
                    return Ok(_mapper.Map<Customer, CustomerDto>(customer_2));
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
