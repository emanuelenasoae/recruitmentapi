using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentAppAPI.Controllers.DTO;
using RecruitmentApp.Entities;

namespace RecruitmentAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public MembersController(IMemberService memberService, IMapper _mapper)
        {
            this._memberService = memberService;
            this._mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberById(int id)
        {
            Member? member = await _memberService.GetMemberByIdAsync(id);

            if (member == null)
            {
                return NotFound($"404NotFound. There is no existing member with id {id}");
            }

            var memberDto = _mapper.Map<MemberDto>(member);

            return Ok(memberDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MemberDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MemberDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto)
        {
            var member = _mapper.Map<Member>(memberDto);

            await _memberService.Create(member);

            return CreatedAtAction("GetMemberById", new { member.Id }, memberDto);
        }
    }
}
