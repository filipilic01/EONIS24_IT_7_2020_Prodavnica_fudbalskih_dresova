using API.Dtos.OrderItems;
using API.Dtos.Orders;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IGenericRepository<OrderItem> _repository;
        private readonly IMapper _mapper;

        public OrderItemController(IGenericRepository<OrderItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems()
        {
            var spec = new OrderItemWithOrderAndJerseySizeSpecification();

            var items = await _repository.ListAsync(spec);

            if (items == null || items.Count == 0)
            {
                return NoContent();
            }

            var orderItemsDto = _mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemDto>>(items);
            return Ok(orderItemsDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{orderItemId}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemById(Guid orderItemId)
        {
            var spec = new OrderItemWithOrderAndJerseySizeSpecification(orderItemId);
            var orderItem = await _repository.GetEntityWithSpec(spec);

            if (orderItem == null)
                return NotFound(new ApiResponse(404, "Order item with ID " + orderItemId + " not found"));

            return _mapper.Map<OrderItem, OrderItemDto>(orderItem);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderItemDto>> AddOrderItem([FromBody] OrderItemCreationDto orderItemPost)
        {
            try
            {
                OrderItem orderItemEntity = _mapper.Map<OrderItem>(orderItemPost);
                orderItemEntity.OrderItemId = Guid.NewGuid();

                await _repository.AddAsync(orderItemEntity);
                var spec = new OrderItemWithOrderAndJerseySizeSpecification(orderItemEntity.OrderId);
                var item = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<OrderItem, OrderItemDto>(item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Creating error"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{orderItemId}")]

        public async Task<IActionResult> DeleteOrderItem(Guid orderItemId)
        {
            try
            {
                var orderItem = await _repository.GetByIdAsync(orderItemId);

                if (orderItem == null)
                {

                    return NotFound(new ApiResponse(404, "Order item with ID " + orderItemId + " not found"));
                }

                await _repository.DeleteAsync(orderItemId);


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
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem([FromBody] OrderItemUpdateDto orderItemUpdate)
        {
            try
            {
                var orderItemEntity = await _repository.GetByIdAsync(orderItemUpdate.OrderItemId);

                if (orderItemEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Order item with " + orderItemUpdate.OrderItemId + " not found"));
                }

                OrderItem orderItem = _mapper.Map<OrderItem>(orderItemUpdate);

                var updateOrderItem = await _repository.UpdateAsync(orderItem, orderItemEntity, (existingOrderItem, newOrderItem) =>
                {
                    existingOrderItem.OrderItemId = newOrderItem.OrderItemId;
                    existingOrderItem.ItemNumber = newOrderItem.ItemNumber;


                    return existingOrderItem;
                });
                var spec = new OrderItemWithOrderAndJerseySizeSpecification(orderItem.OrderItemId);
                var orderItem_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<OrderItem, OrderItemDto>(orderItem_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }
    }
}
