using API.Dtos.Jerseys;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JerseyController : ControllerBase
    {
        private readonly IGenericRepository<Jersey> _repository;
        private readonly IMapper _mapper;

        public JerseyController(IGenericRepository<Jersey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<JerseyDto>>> GetJerseys([FromQuery] JerseySpecParams specParams)
        {
            var spec = new JerseysWithAdminSpecification(specParams);

            var countSpec = new JerseyWithFiltersForCountSpecification(specParams);

            var totalItems = await _repository.CountAsync(countSpec);

            var jerseys = await _repository.ListAsync(spec);

            var data = _mapper.Map<List<Jersey>, List<JerseyDto>>(jerseys);

            return Ok(new Pagination<JerseyDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{jerseyId}")]
        [AllowAnonymous]
        public async Task<ActionResult<JerseyDto>> GetJerseyById(Guid jerseyId)
        {
            var spec = new JerseysWithAdminSpecification(jerseyId);
            var jersey = await _repository.GetEntityWithSpec(spec);

            if (jersey == null)
                return NotFound(new ApiResponse(404, "Jersey with ID " + jerseyId + " not found"));

            return _mapper.Map<Jersey, JerseyDto>(jersey);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<JerseyDto>> AddJersey([FromBody] JerseyCreationDto jerseyPost)
        {
            try
            {
                Jersey jerseyEntity = _mapper.Map<Jersey>(jerseyPost);
                jerseyEntity.JerseyId = Guid.NewGuid();

                await _repository.AddAsync(jerseyEntity);
                var spec = new JerseysWithAdminSpecification(jerseyEntity.JerseyId);
                var jersey = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Jersey, JerseyDto>(jersey));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Creating error"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{jerseyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJersey(Guid jerseyId)
        {
            try
            {
                var jersey = await _repository.GetByIdAsync(jerseyId);

                if (jersey == null)
                {

                    return NotFound(new ApiResponse(404, "Jersey with ID " + jerseyId + " not found"));
                }

                await _repository.DeleteAsync(jerseyId);


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
        public async Task<ActionResult<JerseyDto>> UpdateJersey([FromBody] JerseyUpdateDto jerseyUpdate)
        {
            try
            {
                var jerseyEntity = await _repository.GetByIdAsync(jerseyUpdate.JerseyId);

                if (jerseyEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Jersey with " + jerseyUpdate.JerseyId + " not found"));
                }

                Jersey jersey = _mapper.Map<Jersey>(jerseyUpdate);

                var updateJersey = await _repository.UpdateAsync(jersey, jerseyEntity, (existingJersey, newJersey) =>
                {
                    existingJersey.JerseyId = newJersey.JerseyId;
                    existingJersey.PlayerName = newJersey.PlayerName;
                    existingJersey.Season = newJersey.Season;
                    existingJersey.Team = newJersey.Team;
                    existingJersey.Brand = newJersey.Brand;
                    existingJersey.Price = newJersey.Price;
                    existingJersey.ImageUrl = newJersey.ImageUrl;
                    existingJersey.Type = newJersey.Type;
                    existingJersey.Country = newJersey.Country;
                    existingJersey.Competition = newJersey.Competition;
                    existingJersey.Status = newJersey.Status;
                    existingJersey.TeamUrl = newJersey.TeamUrl;
                    return existingJersey;
                });
                var spec = new JerseysWithAdminSpecification(jersey.JerseyId);
                var jersey_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Jersey, JerseyDto>(jersey_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Updating error"));
            }

        }
    }
}
