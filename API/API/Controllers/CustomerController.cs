using API.Dtos.Admins;
using API.Dtos.Customers;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                var existingCustomer = _repository.GetUsername(customerPost.CustomerUserName, "Customer");
                if (existingCustomer.Equals(true))
                    return BadRequest(new ApiResponse(400, "Username " + customerPost.CustomerUserName + " already exixsts!"));
                Customer customerEntity = _mapper.Map<Customer>(customerPost);
                customerEntity.CustomerId = Guid.NewGuid();
                customerEntity.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customerPost.CustomerPassword);

                await _repository.AddAsync(customerEntity);


                return Ok(_mapper.Map<Customer, CustomerDto>(customerEntity));
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
                var customerEntity = await _repository.GetByIdAsync(customerUpdate.CustomerId);

                if (customerEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Customer with " + customerUpdate.CustomerId + " not found"));
                }

                Customer customer = _mapper.Map<Customer>(customerUpdate);

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
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }

    }
}
