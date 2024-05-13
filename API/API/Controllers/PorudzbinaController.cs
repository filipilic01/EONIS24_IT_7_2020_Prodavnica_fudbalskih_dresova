using API.Dtos.Dres;
using API.Dtos.VelicinaDresa;
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
    public class PorudzbinaController : ControllerBase
    {
        private readonly IGenericRepository<Porudzbina> _repository;
        private readonly IMapper _mapper;

        public PorudzbinaController(IGenericRepository<Porudzbina> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PorudzbinaDto>>> GetPorudzbinas()
        {
            var spec = new PorudzbinaWithKupacSpecification();

            var porudzbinas = await _repository.ListAsync(spec);

            if (porudzbinas == null || porudzbinas.Count == 0)
            {
                return NoContent();
            }

            var porudzbinasDto = _mapper.Map<IEnumerable<Porudzbina>, IEnumerable<PorudzbinaDto>>(porudzbinas);
            return Ok(porudzbinasDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{porudzbinaId}")]
        [Authorize(Roles = "Admin, Kupac")]
        public async Task<ActionResult<PorudzbinaDto>> GetPorudzbinaById(Guid porudzbinaId)
        {
            var spec = new PorudzbinaWithKupacSpecification(porudzbinaId);
            var porudzbina = await _repository.GetEntityWithSpec(spec);

            if (porudzbina == null)
                return NotFound(new ApiResponse(404, "Porudzbina sa ID " + porudzbinaId + " ne postoji"));

            return _mapper.Map<Porudzbina, PorudzbinaDto>(porudzbina);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Kupac")]
        public async Task<ActionResult<PorudzbinaDto>> AddPorudzbina([FromBody] PorudzbinaCreationDto porudzbinaPost)
        {
            try
            {
                Porudzbina porudzbinaEntity = _mapper.Map<Porudzbina>(porudzbinaPost);
                porudzbinaEntity.PorudzbinaId = Guid.NewGuid();

                await _repository.AddAsync(porudzbinaEntity);
                var spec = new PorudzbinaWithKupacSpecification(porudzbinaEntity.PorudzbinaId);
                var porudzbina = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Porudzbina, PorudzbinaDto>(porudzbina));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja porudzbine"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{porudzbinaId}")]
        [Authorize(Roles = "Kupac")]

        public async Task<IActionResult> DeletePorudzbina(Guid porudzbinaId)
        {
            try
            {
                var Porudzbina = await _repository.GetByIdAsync(porudzbinaId);

                if (Porudzbina == null)
                {

                    return NotFound(new ApiResponse(404, "Porudzbina sa ID " + porudzbinaId + " ne postoji"));
                }

                await _repository.DeleteAsync(porudzbinaId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja porudzbine"));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = " Kupac")]
        public async Task<ActionResult<PorudzbinaDto>> UpdatePorudzbina([FromBody] PorudzbinaUpdateDto porudzbinaUpdate)
        {
            try
            {
                var porudzbinaEntity = await _repository.GetByIdAsync(porudzbinaUpdate.PorudzbinaId);

                if (porudzbinaEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Porudzbina sa " + porudzbinaUpdate.PorudzbinaId + " ne postoji"));
                }

                Porudzbina porudzbina = _mapper.Map<Porudzbina>(porudzbinaUpdate);

                var updatePorudzbina = await _repository.UpdateAsync(porudzbina, porudzbinaEntity, (existingPorudzbina, newPorudzbina) =>
                {
                    existingPorudzbina.PorudzbinaId = newPorudzbina.PorudzbinaId;
                    existingPorudzbina.DatumPorudzbine = newPorudzbina.DatumPorudzbine;
                    existingPorudzbina.UkupanIznos = newPorudzbina.UkupanIznos;


                    return existingPorudzbina;
                });
                var spec = new PorudzbinaWithKupacSpecification(porudzbina.PorudzbinaId);
                var porudzbina_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<Porudzbina, PorudzbinaDto>(porudzbina_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja porudzbine"));
            }

        }

        [HttpGet("Kupac/{KupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(Roles = " Kupac")]
        public async Task<ActionResult<List<PorudzbinaDto>>> GetPorudzbinasByKupacId(Guid KupacId)
        {
            var Porudzbinas = await _repository.GetPorudzbinasByKupacId(KupacId);

            if (Porudzbinas.Count==0)
                return NotFound(new ApiResponse(404, "Kupac with ID " + KupacId + " not found"));

            var PorudzbinasDto = _mapper.Map<IEnumerable<Porudzbina>, IEnumerable<PorudzbinaDto>>(Porudzbinas);
            return Ok(PorudzbinasDto.ToList());
        }
    }
}
