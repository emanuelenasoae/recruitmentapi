using Microsoft.IdentityModel.Tokens;
using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentApp.Entities;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentAppAPI.Authentication.Abstractions;

namespace RecruitmentAppAPI.Authentication.Login
{
    public class LoginHandler : ILoginHandler
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IJwtProvider _jwtProvider;

        public LoginHandler(IMemberService memberService, ILogger<LoginHandler> logger, IJwtProvider jwtProvider)
        {
            _memberService = memberService;
            _logger = logger;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Authenticate(string email)
        {

            //Get member
            Member? member = await _memberService.GetMemberByEmailAsync(email);

            if(member == null)
            {
                return string.Empty;
            }

            //Generate JWT
            string token = _jwtProvider.Generate(member);

            // Return JWT
            return token;
        }
    }
}
