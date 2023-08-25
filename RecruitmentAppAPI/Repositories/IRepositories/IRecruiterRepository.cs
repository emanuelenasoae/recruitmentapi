using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.IRepositories
{
    public interface IRecruiterRepository
    {
        Task<Recruiter?> GetRecruiterByIdAsync(int id);
        Task<List<Recruiter>> GetByFirstNameAsync(string firstName);
        Task<IEnumerable<Recruiter?>> GetAllRecruitersAsync();
        void Add(Recruiter recruiter);
        void Update(Recruiter recruiter);
        void Remove(Recruiter recruiter);
        bool RecruiterExists(int id);

    }
}
