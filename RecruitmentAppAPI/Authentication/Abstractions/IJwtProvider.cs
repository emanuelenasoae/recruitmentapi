using RecruitmentApp.Entities;

namespace RecruitmentAppAPI.Authentication.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(Member member);
    }
}
