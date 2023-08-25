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
    public class OpenRolesController : ControllerBase
    {
        private readonly IOpenRoleService _openRoleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OpenRolesController(IOpenRoleService openRoleService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _openRoleService = openRoleService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/OpenRoles
        [HttpGet]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetOpenRoles()
        {
            var openRoles = _mapper.Map<List<OpenRoleDto>>(await _openRoleService.GetAllOpenRolesAsync());

            if(openRoles == null || openRoles.Count == 0)
                return NotFound("There are no available open roles");

            return Ok(openRoles);
        }

        // GET api/OpenRoles/GetById/1
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_unitOfWork.OpenRoles.OpenRoleExists(id))
                return NotFound($"404NotFound. There is not existing open role with id {id}");

            var openRole = _mapper.Map<OpenRoleDto>(await _openRoleService.GetOpenRoleByIdAsync(id));

            return Ok(openRole);
        }

        // GET api/OpenRoles/GetByRoleTitle/roleTitle
        [HttpGet("[action]/{roleTitle}")]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByRoleTitle(string roleTitle)
        {
            var openRoles = await _openRoleService.GetOpenRolesByRoleTitle(roleTitle);

            if (openRoles == null || openRoles.Count == 0)
                return NotFound($"404NotFound. There is no existing open role with role title {roleTitle}");

            var openRolesDto = _mapper.Map<List<OpenRoleDto>>(openRoles);
            return Ok(openRolesDto);
        }

        // POST api/OpenRoles
        [HttpPost]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOpenRole([FromBody] OpenRoleDto openRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var openRole = _mapper.Map<OpenRole>(openRoleDto);

            var validationErrors = await _openRoleService.CreateOpenRoleAsync(openRole);

            foreach(var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return CreatedAtAction("GetById", new { id = openRole.RoleId }, openRoleDto);
        }

        // PUT api/OpenRoles/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateOpenRole(int id, [FromBody] OpenRoleDto updatedOpenRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /*if (id != updatedOpenRoleDto.RoleId)
               return BadRequest(ModelState);*/ //might be needed later, id not part of Dto as of now

            if (!_unitOfWork.OpenRoles.OpenRoleExists(id))
                return NotFound($"There is no existing open role with id {id}");

            var openRole = _mapper.Map<OpenRole>(updatedOpenRoleDto);
            openRole.RoleId= id;

            var validationErrors = await _openRoleService.UpdateOpenRoleAsync(openRole);

            foreach(var error in validationErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(updatedOpenRoleDto);
        }

        // DELETE api/OpenRoles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(OpenRoleDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SoftDeleteOpenRole(int id)
        {
            if (!_unitOfWork.OpenRoles.OpenRoleExists(id))
                return NotFound($"404NotFound. There is no existing open role with role id {id}");

            await _openRoleService.SoftDeleteOpenRoleAsync(id);

            return NoContent();
        }

    }
}
