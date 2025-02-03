namespace RecruitmentAppAPI.Authentication.Abstractions
{
    public interface ILoginHandler
    {
        Task<string> Authenticate(string email);
    }
}