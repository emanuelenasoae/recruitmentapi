using RecruitmentApp.Entities;

namespace RecruitmentAppAPI.Repositories.IRepositories
{
    public interface IMemberRepository
    {
        void Create(Member member);
        Task<Member?> GetMemberByIdAsync(int id);
    }
}
