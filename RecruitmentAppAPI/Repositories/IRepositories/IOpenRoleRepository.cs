using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Repositories.IRepositories
{
    public interface IOpenRoleRepository
    {
        Task<OpenRole?> GetOpenRoleByIdAsync(int id);
        Task<List<OpenRole>> FindByAsync(Expression<Func<OpenRole, bool>> expression);
        Task<IEnumerable<OpenRole?>> GetAllOpenRolesAsync();
        void Add(OpenRole role);
        void Update(OpenRole role);
        void Remove(OpenRole role);
        void SoftDelete(OpenRole role);
        bool OpenRoleExists(int openRoleId);
    }
}
