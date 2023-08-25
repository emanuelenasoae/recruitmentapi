using FluentValidation.Results;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesAbstractions
{
    public  interface IRecruiterService
    {
        Task<Recruiter?> GetRecruiterByIdAsync(int recruiterId);
        Task<List<Recruiter>> GetRecruitersByFirstName(string firstName);
        Task<IEnumerable<Recruiter?>> GetAllRecruitersAsync();
        Task<List<ValidationFailure>> CreateRecruiterAsync(Recruiter recruiter);
        Task<List<ValidationFailure>> UpdateRecruiterAsync(Recruiter recruiter);
    }
}
