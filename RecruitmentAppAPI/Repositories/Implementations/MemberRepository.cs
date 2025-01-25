using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Entities;
using RecruitmentAppAPI.Repositories.IRepositories;

namespace RecruitmentApp.Repositories.Concretions
{
    public class MemberRepository : IMemberRepository
    {
        private readonly RecruitmentContext _dbContext;

        public MemberRepository(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Member member)
        {
            _dbContext.Members.Add(member);
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
