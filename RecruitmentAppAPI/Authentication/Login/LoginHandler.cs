using Microsoft.IdentityModel.Tokens;
using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentApp.Entities;

namespace RecruitmentAppAPI.Authentication.Login
{
    public class LoginHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(IUnitOfWork unitOfWork, ILogger<LoginHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /*public async Task<string> Authenticate (string email)
        {

            //Get member
            Member? member = await _unitOfWork.Members.GetMemberAsync(email);

            if(member == null)
            {
                return string.Empty;
            }

            //Generate JWT

            // Return JWT
        }*/
    }
}
