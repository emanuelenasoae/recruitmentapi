using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentAppAPI.Controllers.DTO;

namespace RecruitmentAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitersController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecruitersController(IRecruiterService recruiterService, IUnitOfWork UnitOfWork , IMapper mapper)
        {
            _recruiterService = recruiterService;
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        // GET: api/Recruiters
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecruiterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<RecruiterDto>), StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetRecruiters()
        {
            var recruiters = _mapper.Map<List<RecruiterDto>>(await _recruiterService.GetAllRecruitersAsync());

            if (recruiters == null || recruiters.Count == 0)
                return NotFound("There are no available recruiters");

            return Ok(recruiters);
        }

        // GET: api/Recruiters/GetById/5
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_unitOfWork.Recruiters.RecruiterExists(id))
                return NotFound($"404NotFound. There is no existing recruiter with id {id}");

            var recruiter = _mapper.Map<RecruiterDto>(await _recruiterService.GetRecruiterByIdAsync(id));

            return Ok(recruiter);
        }

        //GET: api/Recruiters/GetByFirstName/firstName
        [HttpGet("[action]/{firstName}")]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByFirstName(string firstName)
        {
            var recruiter = await _recruiterService.GetRecruitersByFirstName(firstName);

            if(recruiter == null || recruiter.Count == 0)
                return NotFound($"404NotFound. There is no existing recruiter with first name {firstName}");

            var recruitersDto = _mapper.Map<List<RecruiterDto>>(recruiter);
            return Ok(recruitersDto);
        }

        // POST: api/Recruiters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecruiter([FromBody] RecruiterDto recruiterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recruiter = _mapper.Map<Recruiter>(recruiterDto);

            var validationErrors = await _recruiterService.CreateRecruiterAsync(recruiter);

            foreach(var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return CreatedAtAction("GetById", new { id = recruiter.RecruiterId }, recruiterDto);

        }

        // PUT: api/Recruiters/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RecruiterDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRecruiter(int id, [FromBody] RecruiterDto updatedRecruiterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /*if (id != updatedRecruiterDto.RecruiterId)
                return BadRequest(ModelState);*/ //might be needed later, id not part of Dto as of now

            if (!_unitOfWork.Recruiters.RecruiterExists(id))
                return NotFound($"There is no existing recruiter with id {id}");

            var recruiter = _mapper.Map<Recruiter>(updatedRecruiterDto);
            recruiter.RecruiterId = id;

            var validationErrors = await _recruiterService.UpdateRecruiterAsync(recruiter);

            foreach (var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(updatedRecruiterDto);

        }
    }
}
