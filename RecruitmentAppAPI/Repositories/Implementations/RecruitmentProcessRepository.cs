using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.Concretions
{
    public class RecruitmentProcessRepository : IRecruitmentProcessRepository
    {
        private readonly RecruitmentContext _dbContext;

        public RecruitmentProcessRepository(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RecruitmentProcess?> GetRecruitmentProcessByIdAsync(int id)
        {
            return await _dbContext.RecruitmentProcesses
                .Include(c => c.Candidate)
                .Include(r => r.Recruiter)
                .FirstOrDefaultAsync(p => p.ProcessId == id);
        }

        public async Task<List<RecruitmentProcess>> FindByAsync(Expression<Func<RecruitmentProcess, bool>> expression)
        {
            return await _dbContext.RecruitmentProcesses
                .Include(c => c.Candidate)
                .Include(r => r.Recruiter)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<IEnumerable<RecruitmentProcess?>> GetAllRecruitmentProcessesAsync()
        {
            return await _dbContext.RecruitmentProcesses
                .Include(c => c.Candidate)
                .Include(r => r.Recruiter)
                .ToListAsync();
        }

        public void Add(RecruitmentProcess process)
        {
            _dbContext.RecruitmentProcesses.Add(process);
        }

        public void Update(RecruitmentProcess process)
        {
            _dbContext.RecruitmentProcesses.Update(process);
        }

        public void Remove(RecruitmentProcess process)
        {
            _dbContext.RecruitmentProcesses.Remove(process);
        }

        public bool RecruitmentProcessExists(int recruitmentProcessId)
        {
            return _dbContext.RecruitmentProcesses.Any(r => r.ProcessId == recruitmentProcessId);
        }
    }
}
