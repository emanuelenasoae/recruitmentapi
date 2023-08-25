using RecruitmentApp.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICandidateRepository Candidates { get; }
        IOpenRoleRepository OpenRoles { get; }
        IRecruiterRepository Recruiters { get; }
        IRecruitmentProcessRepository RecruitmentProcesses { get; }
        Task<int> SaveChangesAsync();
    }
}
