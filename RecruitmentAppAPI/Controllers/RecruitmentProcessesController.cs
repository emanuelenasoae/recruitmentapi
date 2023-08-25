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
    public class RecruitmentProcessesController : ControllerBase
    {
        private readonly IRecruitmentProcessService _recruitmentProcessService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecruitmentProcessesController(IRecruitmentProcessService recruitmentProcessService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _recruitmentProcessService = recruitmentProcessService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/RecruitmentProcesses
        [HttpGet]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecruitmentProcesses()
        {
            var recruitmentProcesses = _mapper.Map<List<RecruitmentProcessDto>>(await _recruitmentProcessService.GetAllRecruitmentProcessesAsync());

            if (recruitmentProcesses == null || recruitmentProcesses.Count == 0)
                return NotFound("There are no available recruitment processes");

            return Ok(recruitmentProcesses);
        }

        // GET api/RecruitmentProcesses/GetById/1
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_unitOfWork.RecruitmentProcesses.RecruitmentProcessExists(id))
                return NotFound($"404NotFound. There is not existing recruitment process with id {id}");

            var recruitmentProcess = _mapper.Map<RecruitmentProcessDto>(await _recruitmentProcessService.GetRecruitmentProcessById(id));

            return Ok(recruitmentProcess);
        }

        // GET api/RecruitmentProcesses/GetByCandidate/id
        [HttpGet("[action]/{candidateId}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCandidate(int candidateId)
        {
            var recruitmentProcesses = await _recruitmentProcessService.GetRecruitmentProcessesByCandidateId(candidateId);

            if (recruitmentProcesses == null || recruitmentProcesses.Count == 0)
                return NotFound($"404NotFound. There is no existing recruitment process for candidate id {candidateId}");

            var recruitmentProcessesDto = _mapper.Map<List<RecruitmentProcessDto>>(recruitmentProcesses);
            return Ok(recruitmentProcessesDto);
        }

        // GET api/RecruitmentProcesses/GetByRecruiter/id
        [HttpGet("[action]/{recruiterId}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByRecruiter(int recruiterId)
        {
            var recruitmentProcesses = await _recruitmentProcessService.GetRecruitmentProcessesByCandidateId(recruiterId);

            if (recruitmentProcesses == null || recruitmentProcesses.Count == 0)
                return NotFound($"404NotFound. There is no existing recruitment process for recruiter id {recruiterId}");

            var recruitmentProcessesDto = _mapper.Map<List<RecruitmentProcessDto>>(recruitmentProcesses);
            return Ok(recruitmentProcessesDto);
        }

        // POST api/RecruitmentProcesses
        [HttpPost]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecruitmentProcess([FromBody] RecruitmentProcessDto recruitmentProcessDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recruitmentProcess = _mapper.Map<RecruitmentProcess>(recruitmentProcessDto);

            var validationErrors = await _recruitmentProcessService.CreateRecruitmentProcessAsync(recruitmentProcess);

            foreach (var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return CreatedAtAction("GetById", new { id = recruitmentProcess.ProcessId }, recruitmentProcessDto);
        }

        // PUT api/RecruitmentProcesses/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRecruitmentProcess(int id, [FromBody] RecruitmentProcessDto updatedRecruitmentProcessDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /*if (id != updatedRecruitmentProcessDto.ProcessId)
               return BadRequest(ModelState);*/ //might be needed later, id not part of Dto as of now

            if (!_unitOfWork.RecruitmentProcesses.RecruitmentProcessExists(id))
                return NotFound($"There is no existing recruitment process with id {id}");

            var recruitmentProcess = _mapper.Map<RecruitmentProcess>(updatedRecruitmentProcessDto);
            recruitmentProcess.ProcessId = id;

            var validationErrors = await _recruitmentProcessService.UpdateRecruitmentProcessAsync(recruitmentProcess);

            foreach (var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(updatedRecruitmentProcessDto);
        }

        // DELETE api/RecruitmentProcesses/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RecruitmentProcessDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecruitmentProcess(int id)
        {
            if (!_unitOfWork.RecruitmentProcesses.RecruitmentProcessExists(id))
                return NotFound($"404NotFound. There is no existing recruitment process with id {id}");

            await _recruitmentProcessService.DeleteRecruitmentProcessAsync(id);

            return NoContent();
        }
    }
}
