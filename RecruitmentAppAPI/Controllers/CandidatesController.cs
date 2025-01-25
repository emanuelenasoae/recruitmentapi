using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.BAL.ServicesImplementation;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentAppAPI.Controllers.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecruitmentAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CandidatesController(ICandidateService candidateService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _candidateService = candidateService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/Candidates
        [HttpGet]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = _mapper.Map<List<CandidateDto>>(await _candidateService.GetAllCandidatesAsync());

            if (candidates == null || candidates.Count == 0)
            {
                return NotFound("There are no available candidates");
            }

            return Ok(candidates);
        }

        // GET api/Candidates/GetById/1
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_unitOfWork.Candidates.CandidateExists(id))
                return NotFound($"404NotFound. There is no existing candidate with id {id}");

            var candidate = _mapper.Map<CandidateDto>(await _candidateService.GetCandidateByIdAsync(id));

            return Ok(candidate);
        }

        // POST api/Candidates
        [HttpPost]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var candidate = _mapper.Map<Candidate>(candidateDto);

            var validationErrors = await _candidateService.CreateCandidateAsync(candidate);

            foreach(var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return CreatedAtAction("GetById", new { id = candidate.CandidateId }, candidateDto);
        }

        // PUT api/Candidates/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateCandidate(int id, [FromBody] CandidateDto updatedCandidateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            /*if (id != updatedCandidateDto.CandidateId)
               return BadRequest(ModelState);*/ //might be needed later, id not part of Dto as of now

            if (!_unitOfWork.Candidates.CandidateExists(id))
                return NotFound($"There is no existing candiadte with candidate id {id}");

            var candidate = _mapper.Map<Candidate>(updatedCandidateDto);
            candidate.CandidateId= id;

            var validationErrors = await _candidateService.UpdateCandidateAsync(candidate);

            foreach (var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(updatedCandidateDto);
        }

        // DELETE api/Candidates/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SoftDeleteCandidate(int id)
        {
            if (!_unitOfWork.Candidates.CandidateExists(id))
                return NotFound($"404NotFound. There is no existing candidate with candidate id {id}");

            await _candidateService.SoftDeleteCandidateAsync(id);

            return NoContent();
        }
    }
}
