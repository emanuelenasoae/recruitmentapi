using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentAppAPI.Controllers.DTO;
using RecruitmentApp.Entities;
using RecruitmentAppAPI.Authentication.Login;
using RecruitmentAppAPI.Authentication.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace RecruitmentAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        private readonly ILoginHandler _loginHandler;

        public MembersController(IMemberService memberService, IMapper mapper, ILoginHandler loginHandler)
        {
            _memberService = memberService;
            _mapper = mapper;
            _loginHandler = loginHandler;
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

        // GET api/Members/GetMemberById/1
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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

        // GET api/Members/GetMemberByEmail/email@domain.com
        [Authorize]
        [HttpGet("[action]/{email}")]
        [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberByEmail(string email)
        {
            Member? member = await _memberService.GetMemberByEmailAsync(email);

            if (member == null)
            {
                return NotFound($"404NotFound. There is no existing member with email {email}");
            }

            var memberDto = _mapper.Map<MemberDto>(member);

            return Ok(memberDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginMember([FromBody] LoginRequest request)
        {
            string tokenResult = await _loginHandler.Authenticate(request.Email);

            if(tokenResult == string.Empty)
            {
                return BadRequest($"{request.Email} is unauthorized.");
            }

            return Ok(tokenResult);
        }



    }
}
