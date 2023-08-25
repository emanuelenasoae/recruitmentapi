using FluentValidation.Results;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesAbstractions
{
    public interface ICandidateService
    {
        Task<Candidate?> GetCandidateByIdAsync(int candidateId);
        Task<IEnumerable<Candidate?>> GetAllCandidatesAsync();
        Task<List<ValidationFailure>> CreateCandidateAsync(Candidate candidate);
        Task<List<ValidationFailure>> UpdateCandidateAsync(Candidate candidate);
        Task<int> SoftDeleteCandidateAsync(int candidateId);
    }
}