using FluentValidation.Results;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesAbstractions
{
    public  interface IOpenRoleService
    {
        Task<OpenRole?> GetOpenRoleByIdAsync(int openRoleId);
        Task<List<OpenRole>> GetOpenRolesByRoleTitle(string roleTitle);
        Task<IEnumerable<OpenRole?>> GetAllOpenRolesAsync();
        Task<List<ValidationFailure>> CreateOpenRoleAsync(OpenRole role);
        Task<List<ValidationFailure>> UpdateOpenRoleAsync(OpenRole role);
        Task<int> SoftDeleteOpenRoleAsync(int openRoleId);
    }
}
