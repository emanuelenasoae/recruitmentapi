using RecruitmentApp.Entities;

namespace RecruitmentApp.BAL.ServicesAbstractions
{
    public interface IMemberService
    {
        Task<int> Create(Member member);
        Task<Member?> GetMemberByIdAsync(int id);
    }
}