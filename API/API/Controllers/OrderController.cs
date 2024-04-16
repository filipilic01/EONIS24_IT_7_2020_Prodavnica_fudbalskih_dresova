using API.Dtos.Jerseys;
using API.Dtos.JerseySizes;
using API.Dtos.OrderItems;
using API.Dtos.Orders;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IGenericRepository<Order> _repository;
        private readonly IMapper _mapper;

        public OrderController(IGenericRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var spec = new OrderWithCustomerSpecification();

            var orders = await _repository.ListAsync(spec);

            if (orders == null || orders.Count == 0)
            {
                return NoContent();
            }

            var ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
        {
            var spec = new OrderWithCustomerSpecification(orderId);
            var order = await _repository.GetEntityWithSpec(spec);

            if (order == null)
                return NotFound(new ApiResponse(404, "Order with ID " + orderId + " not found"));

            return _mapper.Map<Order, OrderDto>(order);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<OrderDto>> AddOrder([FromBody] OrderCreationDto orderPost)
        {
            try
            {
                Order orderEntity = _mapper.Map<Order>(orderPost);
                orderEntity.OrderId = Guid.NewGuid();

                await _repository.AddAsync(orderEntity);
                var spec = new OrderWithCustomerSpecification(orderEntity.OrderId);
                var order = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Order, OrderDto>(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Creating error"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Admin,  Customer")]

        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            try
            {
                var order = await _repository.GetByIdAsync(orderId);

                if (order == null)
                {

                    return NotFound(new ApiResponse(404, "Order with ID " + orderId + " not found"));
                }

                await _repository.DeleteAsync(orderId);


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
        public async Task<ActionResult<OrderDto>> UpdateOrder([FromBody] OrderUpdateDto orderUpdate)
        {
            try
            {
                var orderEntity = await _repository.GetByIdAsync(orderUpdate.OrderId);

                if (orderEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Order with " + orderUpdate.OrderId + " not found"));
                }

                Order order = _mapper.Map<Order>(orderUpdate);

                var updateOrder = await _repository.UpdateAsync(order, orderEntity, (existingOrder, newOrder) =>
                {
                    existingOrder.OrderId = newOrder.OrderId;
                    existingOrder.OrderDate = newOrder.OrderDate;
                    existingOrder.OrderTotalAmount = newOrder.OrderTotalAmount;


                    return existingOrder;
                });
                var spec = new OrderWithCustomerSpecification(order.OrderId);
                var order_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Order, OrderDto>(order_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }

        [HttpGet("Customer/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByCustomerId(Guid customerId)
        {
            var orders = await _repository.GetOrdersByCustomerId(customerId);

            if (orders.Count==0)
                return NotFound(new ApiResponse(404, "Customer with ID " + customerId + " not found"));

            var ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto.ToList());
        }
    }
}
