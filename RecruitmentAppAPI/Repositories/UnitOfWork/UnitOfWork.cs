using Microsoft.Extensions.Logging;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.Concretions;
using RecruitmentApp.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ICandidateRepository? _candidateRepository;

        private IOpenRoleRepository? _openRoleRepository;

        private IRecruiterRepository? _recruiterRepository;

        private IRecruitmentProcessRepository? _recruitmentProcessRepository;

        private readonly RecruitmentContext _dbContext;

        public UnitOfWork(RecruitmentContext dbContext)
        {
            _dbContext= dbContext;
        }

        public ICandidateRepository Candidates
        {
            get
            {
                if (_candidateRepository == null)
                    _candidateRepository = new CandidateRepository(_dbContext);
                return _candidateRepository;
            }
        }

        public IOpenRoleRepository OpenRoles
        {
            get
            {
                if (_openRoleRepository == null)
                    _openRoleRepository= new OpenRoleRepository(_dbContext);
                return _openRoleRepository;
            }
        }

        public IRecruiterRepository Recruiters
        {
            get
            {
                if (_recruiterRepository == null)
                    _recruiterRepository = new RecruiterRepository(_dbContext);
                return _recruiterRepository;
            }
        }

        public IRecruitmentProcessRepository RecruitmentProcesses
        {
            get
            {
                if(_recruitmentProcessRepository == null)
                    _recruitmentProcessRepository= new RecruitmentProcessRepository(_dbContext);
                return _recruitmentProcessRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}