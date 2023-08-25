using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class CandidateRepository : ICandidateRepository
    {
        private readonly RecruitmentContext _dbContext;

        public CandidateRepository(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Candidate?> GetCandidateByIdAsync(int id)
        {
            return await _dbContext.Candidates
                .Include(o => o.OpenRole)
                //.Include(r => r.RecruitmentProcesses)
                .Where(v => v.IsActive == true)
                .FirstOrDefaultAsync(i => i.CandidateId == id);
        }

        public async Task<Candidate?> FindByAsync(Expression<Func<Candidate, bool>> expression)
        {
            return await _dbContext.Candidates
                .Include(o => o.OpenRole)
                .Include(r => r.RecruitmentProcesses)
                .Where(v => v.IsActive == true)
                .FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<Candidate?>> GetAllCandidatesAsync()
        {
            return await _dbContext.Candidates
                .Include(o => o.OpenRole)
                .Include(r => r.RecruitmentProcesses)
                .Where(v => v.IsActive == true)
                .ToListAsync();
        }

        public void Add(Candidate candidate)
        {
            _dbContext.Candidates.Add(candidate);
        }

        public void Update(Candidate candidate)
        {
            _dbContext.Candidates.Update(candidate);
        }

        public void Remove(Candidate candidate)
        {
            _dbContext.Candidates.Remove(candidate);
        }

        public void SoftDelete(Candidate candidate)
        {
            candidate.IsActive= false;
            _dbContext.Candidates.Update(candidate);
        }

        public bool CandidateExists(int candidateId)
        {
            return _dbContext.Candidates.Any(c => c.CandidateId == candidateId);
        }
    }
}
