using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
