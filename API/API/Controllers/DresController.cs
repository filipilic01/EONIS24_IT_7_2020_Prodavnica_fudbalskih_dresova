using API.Dtos.Dres;

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
    public class DresController : ControllerBase
    {
        private readonly IGenericRepository<Dres> _repository;
        private readonly IMapper _mapper;

        public DresController(IGenericRepository<Dres> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<DresDto>>> GetDress([FromQuery] DresSpecParams specParams)
        {
            var spec = new DresWithAdminSpecification(specParams);

            var countSpec = new DresWithFiltersForCountSpecification(specParams);

            var totalItems = await _repository.CountAsync(countSpec);

            var dresovi = await _repository.ListAsync(spec);

            var data = _mapper.Map<List<Dres>, List<DresDto>>(dresovi);

            return Ok(new Pagination<DresDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{dresId}")]
        [AllowAnonymous]
        public async Task<ActionResult<DresDto>> GetDresById(Guid dresId)
        {
            var spec = new DresWithAdminSpecification(dresId);
            var dres = await _repository.GetEntityWithSpec(spec);

            if (dres == null)
                return NotFound(new ApiResponse(404, "Dres sa ID " + dresId + " ne postoji"));

            return _mapper.Map<Dres, DresDto>(dres);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DresDto>> AddDres([FromBody] DresCreationDto DresPost)
        {
            try
            {
                Dres dresEntity = _mapper.Map<Dres>(DresPost);
                dresEntity.DresId = Guid.NewGuid();

                await _repository.AddAsync(dresEntity);
                var spec = new DresWithAdminSpecification(dresEntity.DresId);
                var Dres = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Dres, DresDto>(Dres));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja dresa"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{dresId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDres(Guid dresId)
        {
            try
            {
                var dres = await _repository.GetByIdAsync(dresId);

                if (dres == null)
                {

                    return NotFound(new ApiResponse(404, "Dres sa ID " + dresId + "ne postoji"));
                }

                await _repository.DeleteAsync(dresId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja dresa"));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DresDto>> UpdateDres([FromBody] DresUpdateDto dresUpdate)
        {
            try
            {
                var dresEntity = await _repository.GetByIdAsync(dresUpdate.DresId);

                if (dresEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Dres sa " + dresUpdate.DresId + " ne postoji"));
                }

                Dres dres = _mapper.Map<Dres>(dresUpdate);

                var updateDres = await _repository.UpdateAsync(dres, dresEntity, (existingDres, newDres) =>
                {
                    existingDres.DresId = newDres.DresId;
                    existingDres.ImeIgraca = newDres.ImeIgraca;
                    existingDres.Sezona = newDres.Sezona;
                    existingDres.Tim = newDres.Tim;
                    existingDres.Brend = newDres.Brend;
                    existingDres.Cena = newDres.Cena;
                    existingDres.SlikaUrl = newDres.SlikaUrl;
                    existingDres.Tip = newDres.Tip;
                    existingDres.Zemlja = newDres.Zemlja;
                    existingDres.Takmicenje = newDres.Takmicenje;
                    existingDres.Status = newDres.Status;
                    existingDres.TimUrl = newDres.TimUrl;
                    return existingDres;
                });
                var spec = new DresWithAdminSpecification(dres.DresId);
                var dres_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Dres, DresDto>(dres_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja dresa "));
            }

        }
    }
}
