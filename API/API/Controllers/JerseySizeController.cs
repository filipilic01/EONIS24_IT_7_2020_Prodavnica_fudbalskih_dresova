using API.Dtos.Jerseys;
using API.Dtos.JerseySizes;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JerseySizeController : ControllerBase
    {
        private readonly IGenericRepository<JerseySize> _repository;
        private readonly IMapper _mapper;

        public JerseySizeController(IGenericRepository<JerseySize> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JerseySizeDto>>> GetJerseySizes()
        {
            var spec = new JerseySizeWithJerseySpecification();

            var sizes = await _repository.ListAsync(spec);

            if(sizes == null || sizes.Count == 0) 
            {
                return NoContent();
            }

            var sizesDto = _mapper.Map<IEnumerable<JerseySize>, IEnumerable<JerseySizeDto>>(sizes);
            return Ok(sizesDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{jerseySizeId}")]
        public async Task<ActionResult<JerseySizeDto>> GetJerseySizeById(Guid jerseySizeId)
        {
            var spec = new JerseySizeWithJerseySpecification(jerseySizeId);
            var size = await _repository.GetEntityWithSpec(spec);

            if (size == null)
                return NotFound(new ApiResponse(404, "Jersey size with ID " + jerseySizeId + " not found"));

            return _mapper.Map<JerseySize, JerseySizeDto>(size);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JerseyDto>> AddJerseySize([FromBody] JerseySizeCreationDto jerseySizePost)
        {
            try
            {
                JerseySize jerseySizeEntity = _mapper.Map<JerseySize>(jerseySizePost);
                jerseySizeEntity.JerseySizeId = Guid.NewGuid();

                await _repository.AddAsync(jerseySizeEntity);
                var spec = new JerseySizeWithJerseySpecification(jerseySizeEntity.JerseySizeId);
                var size = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<JerseySize, JerseySizeDto>(size));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Creating error"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{jerseySizeId}")]

        public async Task<IActionResult> DeleteJerseySize(Guid jerseySizeId)
        {
            try
            {
                var size = await _repository.GetByIdAsync(jerseySizeId);

                if (size == null)
                {

                    return NotFound(new ApiResponse(404, "Jersey size with ID " + jerseySizeId + " not found"));
                }

                await _repository.DeleteAsync(jerseySizeId);


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
        public async Task<ActionResult<JerseySizeDto>> UpdateJerseySize([FromBody] JerseySizeUpdateDto jerseySizeUpdate)
        {
            try
            {
                var jerseySizeEntity = await _repository.GetByIdAsync(jerseySizeUpdate.JerseySizeId);

                if (jerseySizeEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Jersey size with " + jerseySizeUpdate.JerseySizeId + " not found"));
                }

                JerseySize size = _mapper.Map<JerseySize>(jerseySizeUpdate);

                var updateJersey = await _repository.UpdateAsync(size, jerseySizeEntity, (existingJerseySize, newJerseySize) =>
                {
                    existingJerseySize.JerseySizeId = newJerseySize.JerseySizeId;
                    existingJerseySize.JerseySizeValue = newJerseySize.JerseySizeValue;
                    existingJerseySize.Quantity = newJerseySize.Quantity;
          
                 
                    return existingJerseySize;
                });
                var spec = new JerseySizeWithJerseySpecification(size.JerseySizeId);
                var size_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<JerseySize, JerseySizeDto>(size_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }
    }
}
