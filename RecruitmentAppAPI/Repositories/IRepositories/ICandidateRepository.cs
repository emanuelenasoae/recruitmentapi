using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.IRepositories
{
    public interface ICandidateRepository
    {
        Task<Candidate?> GetCandidateByIdAsync(int id);
        Task<Candidate?> FindByAsync(Expression<Func<Candidate, bool>> expression);
        Task<IEnumerable<Candidate?>> GetAllCandidatesAsync();
        void Add(Candidate candidate);
        void Update(Candidate candidate);
        void Remove(Candidate candidate);
        void SoftDelete(Candidate candidate);
        bool CandidateExists(int candidateId);

    }
}
