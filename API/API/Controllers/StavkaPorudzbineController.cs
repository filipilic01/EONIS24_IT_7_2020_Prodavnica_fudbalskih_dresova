using API.Dtos.StavkaPorudzbine;
using API.Dtos.Porudzbina;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class StavkaPorudzbineController : ControllerBase
    {
        private readonly IGenericRepository<StavkaPorudzbine> _repository;
        private readonly IMapper _mapper;

        public StavkaPorudzbineController(IGenericRepository<StavkaPorudzbine> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<StavkaPorudzbineDto>>> GetStavkaPorudzbines()
        {
            var spec = new StavkaWithPorudzbinaAndVelicinaDresaSpecification();

            var items = await _repository.ListAsync(spec);

            if (items == null || items.Count == 0)
            {
                return NoContent();
            }

            var StavkaPorudzbinesDto = _mapper.Map<IEnumerable<StavkaPorudzbine>, IEnumerable<StavkaPorudzbineDto>>(items);
            return Ok(StavkaPorudzbinesDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{stavkaPorudzbineId}")]
        [Authorize(Roles = "Kupac")]
        public async Task<ActionResult<StavkaPorudzbineDto>> GetStavkaPorudzbineById(Guid stavkaPorudzbineId)
        {
            var spec = new StavkaWithPorudzbinaAndVelicinaDresaSpecification(stavkaPorudzbineId);
            var stavkaPorudzbine = await _repository.GetEntityWithSpec(spec);

            if (stavkaPorudzbine == null)
                return NotFound(new ApiResponse(404, "Stavka porudzbine sa ID " + stavkaPorudzbineId + " ne postoji"));

            return _mapper.Map<StavkaPorudzbine, StavkaPorudzbineDto>(stavkaPorudzbine);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Kupac")]
        public async Task<ActionResult<StavkaPorudzbineDto>> AddStavkaPorudzbine([FromBody] StavkaPorudzbineCreationDto stavkaPorudzbinePost)
        {
            try
            {
                StavkaPorudzbine stavkaPorudzbineEntity = _mapper.Map<StavkaPorudzbine>(stavkaPorudzbinePost);
                stavkaPorudzbineEntity.StavkaPorudzbineId = Guid.NewGuid();

                await _repository.AddAsync(stavkaPorudzbineEntity);
                var spec = new StavkaWithPorudzbinaAndVelicinaDresaSpecification(stavkaPorudzbineEntity.StavkaPorudzbineId);
                var item = await _repository.GetEntityWithSpec(spec);
                Console.WriteLine(item);
                return CreatedAtAction(nameof(AddStavkaPorudzbine), _mapper.Map<StavkaPorudzbine, StavkaPorudzbineDto>(item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja stavke porudzbine"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{stavkaPorudzbineId}")]
        [Authorize(Roles = "Kupac")]

        public async Task<IActionResult> DeleteStavkaPorudzbine(Guid stavkaPorudzbineId)
        {
            try
            {
                var item = await _repository.GetByIdAsync(stavkaPorudzbineId);

                if (item == null)
                {

                    return NotFound(new ApiResponse(404, "Stavka porudzbine sa ID " + stavkaPorudzbineId + "ne postoji"));
                }

                await _repository.DeleteAsync(stavkaPorudzbineId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja stavke porudzbine"));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Kupac")]
        public async Task<ActionResult<StavkaPorudzbineDto>> UpdateStavkaPorudzbine([FromBody] StavkaPorudzbineUpdateDto stavkaPorudzbineUpdate)
        {
            try
            {
                var stavkaPorudzbineEntity = await _repository.GetByIdAsync(stavkaPorudzbineUpdate.StavkaPorudzbineId);

                if (stavkaPorudzbineEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Stavka porudzbine sa " + stavkaPorudzbineUpdate.StavkaPorudzbineId + " ne postoji"));
                }

                StavkaPorudzbine stavkaPorudzbine = _mapper.Map<StavkaPorudzbine>(stavkaPorudzbineUpdate);

                var updateStavkaPorudzbine = await _repository.UpdateAsync(stavkaPorudzbine, stavkaPorudzbineEntity, (existingStavkaPorudzbine, newStavkaPorudzbine) =>
                {
                    existingStavkaPorudzbine.StavkaPorudzbineId = newStavkaPorudzbine.StavkaPorudzbineId;
                    existingStavkaPorudzbine.BrojStavki = newStavkaPorudzbine.BrojStavki;


                    return existingStavkaPorudzbine;
                });
                var spec = new StavkaWithPorudzbinaAndVelicinaDresaSpecification(stavkaPorudzbine.StavkaPorudzbineId);
                var stavkaPorudzbine_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<StavkaPorudzbine, StavkaPorudzbineDto>(stavkaPorudzbine_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja stavke porudzbine"));
            }

        }
        
        [HttpGet("Porudzbina/{porudzbinaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(Roles = "Admin, Kupac")]
        public async Task<ActionResult<List<StavkaPorudzbineDto>>> GetStavkaPorudzbinesByPorudzbinaId(Guid porudzbinaId)
        {
            var stavkaPorudzbines = await _repository.GetStavkaPorudzbineByPorudzbinaId(porudzbinaId);

            if (stavkaPorudzbines == null)
                return NotFound(new ApiResponse(404, "Porudzbina sa ID " + porudzbinaId + " ne postoji"));

            var stavkaPorudzbinesDto = _mapper.Map<IEnumerable<StavkaPorudzbine>, IEnumerable<StavkaPorudzbineDto>>(stavkaPorudzbines);
            return Ok(stavkaPorudzbines.ToList());
        }
    }
}
