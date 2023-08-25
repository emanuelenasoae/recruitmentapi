using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class OpenRoleRepository : IOpenRoleRepository
    {
        private readonly RecruitmentContext _dbContext;

        public OpenRoleRepository(RecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OpenRole?> GetOpenRoleByIdAsync(int id)
        {
            return await _dbContext.OpenRoles
                //.Include(c => c.Candidates) commented because candidate will enter change tracker for PUT methods (for verify if exists in validators)
                .FirstOrDefaultAsync(o => o.RoleId == id);
        }

        public async Task<List<OpenRole>> FindByAsync(Expression<Func<OpenRole, bool>> expression)
        {
            return await _dbContext.OpenRoles
                .Include(c => c.Candidates)
                .Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<OpenRole?>> GetAllOpenRolesAsync()
        {
            return await _dbContext.OpenRoles
                .Include(c => c.Candidates)
                .ToListAsync();
        }

        public void Add(OpenRole role)
        {
            _dbContext.OpenRoles.Add(role);
        }

        public void Update(OpenRole role)
        {
            _dbContext.OpenRoles.Update(role);
        }

        public void Remove(OpenRole role)
        {
            _dbContext.OpenRoles.Remove(role);
        }

        public void SoftDelete(OpenRole role)
        {
            role.EndDate = DateTime.Now;
            _dbContext.OpenRoles.Update(role);
        }

        public bool OpenRoleExists(int openRoleId)
        {
            return _dbContext.OpenRoles.Any(o => o.RoleId == openRoleId);
        }
    }
}
