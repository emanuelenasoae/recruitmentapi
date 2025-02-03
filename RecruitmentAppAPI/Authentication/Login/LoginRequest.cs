
namespace RecruitmentAppAPI.Authentication.Login
{
    public record LoginRequest
    {
        public string Email { get; init; }

        public LoginRequest(string email)
        {
                Email = email;
        }
    }
}