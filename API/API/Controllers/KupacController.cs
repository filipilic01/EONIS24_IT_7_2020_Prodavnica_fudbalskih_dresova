
using API.Dtos.Kupac;


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
    public class KupacController : ControllerBase
    {
        private readonly IGenericRepository<Kupac> _repository;
        private readonly IMapper _mapper;

        public KupacController(IGenericRepository<Kupac> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<ActionResult<List<KupacDto>>> GetKupacs()
        {

            var kupci = await _repository.GetAllAsync();

            if (kupci == null || kupci.Count == 0)
            {
                return NoContent();
            }

            var KupacsDto = _mapper.Map<IEnumerable<Kupac>, IEnumerable<KupacDto>>(kupci);
            return Ok(KupacsDto.ToList());

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{kupacId}")]
        [Authorize(Roles = "Admin, Kupac")]
        public async Task<ActionResult<KupacDto>> GetKupacById(Guid kupacId)
        {

            var kupac = await _repository.GetByIdAsync(kupacId);

            if (kupac == null)
                return NotFound(new ApiResponse(404, "Kupac sa ID " + kupacId + " nije pronadjen "));

            return _mapper.Map<Kupac, KupacDto>(kupac);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<KupacDto>> AddKupac([FromBody] KupacCreationDto KupacPost)
        {
            try
            {
                if (IsValidEmail(KupacPost.KupacEmail))
                {
                    var existingKupacKorisnickoIme = _repository.GetKorisnickoIme(KupacPost.KupacKorisnickoIme, "Kupac");
                    if (existingKupacKorisnickoIme.Equals(true))
                        return BadRequest(new ApiResponse(400, "Korisničko ime " + KupacPost.KupacKorisnickoIme + " već postoji!"));
                    var existingKupacEmail = _repository.GetEmail(KupacPost.KupacEmail, "Kupac");
                    if (existingKupacEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + KupacPost.KupacEmail + " već postoji!"));
                    Kupac KupacEntity = _mapper.Map<Kupac>(KupacPost);
                    KupacEntity.KupacId = Guid.NewGuid();
                    KupacEntity.KupacLozinka = BCrypt.Net.BCrypt.HashPassword(KupacPost.KupacLozinka);

                    await _repository.AddAsync(KupacEntity);


                    return Ok(_mapper.Map<Kupac, KupacDto>(KupacEntity));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Nepravilan format mejla"));
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom kreiranja kupca"));
            }


        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{kupacId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteKupac(Guid kupacId)
        {
            try
            {
                var kupac = await _repository.GetByIdAsync(kupacId);

                if (kupac == null)
                {

                    return NotFound(new ApiResponse(404, "Kupac sa ID " + kupacId + "nije pronadjen"));
                }

                await _repository.DeleteAsync(kupacId);


                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiException(500, "Greska prilikom brisanja kupca"));
            }


        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Authorize(Roles = "Admin, Kupac")]
        [AllowAnonymous]
        public async Task<ActionResult<KupacDto>> UpdateKupac([FromBody] KupacUpdateDto kupacUpdate)
        {
            try
            {
                
                    var kupacEntity = await _repository.GetByIdAsync(kupacUpdate.KupacId);

                    if (kupacEntity == null)
                    {

                        return NotFound(new ApiResponse(404, "Kupac sa ID " + kupacUpdate.KupacId + " nije pronadjen"));
                    }

                    Kupac Kupac = _mapper.Map<Kupac>(kupacUpdate);
                    var existingKupacKorisnickoIme = _repository.GetKorisnickoIme(Kupac.KupacKorisnickoIme, "Kupac");
                    if (existingKupacKorisnickoIme.Equals(true))
                        return BadRequest(new ApiResponse(400, "KorisnickoIme " + Kupac.KupacKorisnickoIme + " vec postoji!"));
                    var existingKupacEmail = _repository.GetEmail(Kupac.KupacEmail, "Kupac");
                    if (existingKupacEmail.Equals(true))
                        return BadRequest(new ApiResponse(400, "Email " + Kupac.KupacEmail + " vec postoji!"));
                    var updateKupac = await _repository.UpdateAsync(Kupac, kupacEntity, (existingKupac, newKupac) =>
                    {
                        existingKupac.KupacId = newKupac.KupacId;
                        existingKupac.KupacIme = newKupac.KupacIme;
                        existingKupac.KupacPrezime = newKupac.KupacPrezime;
                   
                        
                        existingKupac.KupacBrojTelefona = newKupac.KupacBrojTelefona;
                     
                        existingKupac.KupacAdresa = newKupac.KupacAdresa;

                        return existingKupac;
                    });

                    var kupac_2 = await _repository.GetByIdAsync(kupacUpdate.KupacId);
                    return Ok(_mapper.Map<Kupac, KupacDto>(kupac_2));
               
            }
             
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Greska prilikom editovanja kupca"));
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
