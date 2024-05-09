using API.Dtos.Dres;
using API.Dtos.StavkaPorudzbine;
using API.Dtos.VelicinaDresa;
using API.Errors;
using API.Helpers;
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
    public class VelicinaDresaController : ControllerBase
    {
        private readonly IGenericRepository<VelicinaDresa> _repository;
        private readonly IMapper _mapper;

        public VelicinaDresaController(IGenericRepository<VelicinaDresa> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<List<VelicinaDresaDto>>> GetVelicinaDresas()
        {
            var spec = new VelicinaDresaWithDresSpecification();

            var sizes = await _repository.ListAsync(spec);

            if (sizes == null || sizes.Count == 0)
            {
                return NoContent();
            }

            var sizesDto = _mapper.Map<IEnumerable<VelicinaDresa>, IEnumerable<VelicinaDresaDto>>(sizes);
            return Ok(sizesDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{velicinaDresaId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VelicinaDresaDto>> GetVelicinaDresaById(Guid velicinaDresaId)
        {
            var spec = new VelicinaDresaWithDresSpecification(velicinaDresaId);
            var size = await _repository.GetEntityWithSpec(spec);

            if (size == null)
                return NotFound(new ApiResponse(404, "Velicina dresa sa ID " + velicinaDresaId + " ne postoji"));

            return _mapper.Map<VelicinaDresa, VelicinaDresaDto>(size);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DresDto>> AddDresSize([FromBody] VelicinaDresaCreationDto velicinaDresaPost)
        {
            try
            {
                VelicinaDresa velicinaDresaEntity = _mapper.Map<VelicinaDresa>(velicinaDresaPost);
                velicinaDresaEntity.VelicinaDresaId = Guid.NewGuid();

                await _repository.AddAsync(velicinaDresaEntity);
                var spec = new VelicinaDresaWithDresSpecification(velicinaDresaEntity.VelicinaDresaId);
                var size = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<VelicinaDresa, VelicinaDresaDto>(size));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja velicine dresa"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{velicinaDresaId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteDresSize(Guid velicinaDresaId)
        {
            try
            {
                var size = await _repository.GetByIdAsync(velicinaDresaId);

                if (size == null)
                {

                    return NotFound(new ApiResponse(404, "Velicina dresa with ID " + velicinaDresaId + " ne postoji "));
                }

                await _repository.DeleteAsync(velicinaDresaId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja velicine dresa"));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VelicinaDresaDto>> UpdateDresSize([FromBody] VelicinaDresaUpdateDto velicinaDresaUpdate)
        {
            try
            {
                var velicinaDresaEntity = await _repository.GetByIdAsync(velicinaDresaUpdate.VelicinaDresaId);

                if (velicinaDresaEntity == null)
                {

                    return NotFound(new ApiResponse(404, "Velicina dresa sa " + velicinaDresaUpdate.VelicinaDresaId + " ne postoji"));
                }

                VelicinaDresa size = _mapper.Map<VelicinaDresa>(velicinaDresaUpdate);

                var updateVelicinaDresa = await _repository.UpdateAsync(size, velicinaDresaEntity, (existingVelicinaDresa, newVelicinaDresa) =>
                {
                    existingVelicinaDresa.VelicinaDresaId = newVelicinaDresa.VelicinaDresaId;
                    existingVelicinaDresa.VelicinaDresaVrednost = newVelicinaDresa.VelicinaDresaVrednost;
                    existingVelicinaDresa.Kolicina = newVelicinaDresa.Kolicina;


                    return existingVelicinaDresa;
                });
                var spec = new VelicinaDresaWithDresSpecification(size.VelicinaDresaId);
                var size_2 = await _repository.GetEntityWithSpec(spec);
                return Ok(_mapper.Map<VelicinaDresa, VelicinaDresaDto>(size_2));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja velicine dresa"));
            }

        }

        [HttpGet("Dres/{dresId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<List<VelicinaDresaDto>>> GetVelicineDresovaByIdDres(Guid dresId)
        {
            var velicine = await _repository.GetVelicineDresovaByDresId(dresId);
            
            if (velicine == null)
                return NotFound(new ApiResponse(404, "Dres sa ID " + dresId + " ne postoji"));

            List<VelicinaDresa> list = new List<VelicinaDresa>();
            foreach (VelicinaDresa v in velicine)
            {
                if (v.Kolicina > 0)
                {
                    list.Add(v);
                }
            }
            var velicineDto = _mapper.Map<IEnumerable<VelicinaDresa>, IEnumerable<VelicinaDresaDto>>(list);
            return Ok(velicineDto.ToList());
        }
    }
}
