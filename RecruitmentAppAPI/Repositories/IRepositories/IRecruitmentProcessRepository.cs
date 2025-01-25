using RecruitmentApp.Entities;
using System.Linq.Expressions;

namespace RecruitmentApp.Repositories.IRepositories
{
    public interface IRecruitmentProcessRepository
    {
        Task<RecruitmentProcess?> GetRecruitmentProcessByIdAsync(int id);
        Task<List<RecruitmentProcess>> FindByAsync(Expression<Func<RecruitmentProcess, bool>> expression);
        Task<IEnumerable<RecruitmentProcess?>> GetAllRecruitmentProcessesAsync();
        void Add(RecruitmentProcess process);
        void Update(RecruitmentProcess process);
        void Remove(RecruitmentProcess process);
        bool RecruitmentProcessExists(int recruitmentProcessId);
    }
}
