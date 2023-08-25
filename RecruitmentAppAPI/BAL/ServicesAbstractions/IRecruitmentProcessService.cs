using FluentValidation.Results;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesAbstractions
{
    public interface IRecruitmentProcessService
    {
        Task<RecruitmentProcess?> GetRecruitmentProcessById(int processId);
        Task<List<RecruitmentProcess>> GetRecruitmentProcessesByCandidateId(int candidateId);
        Task<List<RecruitmentProcess>> GetRecruitmentProcessesByRecruiterId(int recruiterId);
        Task<IEnumerable<RecruitmentProcess?>> GetAllRecruitmentProcessesAsync();
        Task<List<ValidationFailure>> CreateRecruitmentProcessAsync(RecruitmentProcess process);
        Task<List<ValidationFailure>> UpdateRecruitmentProcessAsync(RecruitmentProcess process);
        Task<int> DeleteRecruitmentProcessAsync(int processId);
    }
}
