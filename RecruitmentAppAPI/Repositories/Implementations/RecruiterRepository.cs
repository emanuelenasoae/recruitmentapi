using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.Concretions
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly RecruitmentContext _dbContext;

        public RecruiterRepository(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Recruiter?> GetRecruiterByIdAsync(int id)
        {
            return await _dbContext.Recruiters
                //.Include(r => r.RecruitmentProcesses)
                .FirstOrDefaultAsync(i => i.RecruiterId == id);
        }

        public async Task<List<Recruiter>> GetByFirstNameAsync(string firstName)
        {
            return await _dbContext.Recruiters
                .Include(r => r.RecruitmentProcesses)
                .Where(f => f.FirstName == firstName).ToListAsync();
        }

        public async Task<IEnumerable<Recruiter?>> GetAllRecruitersAsync()
        {
            return await _dbContext.Recruiters
                .Include(r => r.RecruitmentProcesses)
                .ToListAsync();
        }

        public void Add(Recruiter recruiter)
        {
            _dbContext.Recruiters.Add(recruiter);
        }

        public void Update(Recruiter recruiter)
        {
            _dbContext.Recruiters.Update(recruiter);
        }

        public void Remove(Recruiter recruiter)
        {
            _dbContext.Recruiters.Remove(recruiter);
        }

        public bool RecruiterExists(int recruiterId)
        {
            return _dbContext.Recruiters.Any(c => c.RecruiterId == recruiterId);
        }
    }
}
