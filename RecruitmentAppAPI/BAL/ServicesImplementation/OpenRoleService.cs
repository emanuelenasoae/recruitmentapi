using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.BAL.Validators;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesImplementation
{
    public class OpenRoleService : IOpenRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OpenRoleValidator _openRoleValidator;
        private readonly ILogger<OpenRoleService> _logger;

        public OpenRoleService(IUnitOfWork unitOfWork,
            OpenRoleValidator openRoleValidator,
            ILogger<OpenRoleService> logger)
        {
            _unitOfWork = unitOfWork;
            _openRoleValidator = openRoleValidator;
            _logger = logger;
        }

        public async Task<OpenRole?> GetOpenRoleByIdAsync(int openRoleId)
        {
            try
            {
                return await _unitOfWork.OpenRoles.GetOpenRoleByIdAsync(openRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch open role with id {openRoleId}");
                throw;
            }
        }

        public async Task<List<OpenRole>> GetOpenRolesByRoleTitle(string roleTitle)
        {
            try
            {
                return await _unitOfWork.OpenRoles.FindByAsync(role => role.RoleTitle == roleTitle);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch open role with role title {roleTitle}");
                throw;
            }
        }

        public async Task<IEnumerable<OpenRole?>> GetAllOpenRolesAsync()
        {
            try
            {
                return await _unitOfWork.OpenRoles.GetAllOpenRolesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying fetch all open roles");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> CreateOpenRoleAsync(OpenRole role)
        {
            try
            {
                ValidationResult results = await _openRoleValidator.ValidateAsync(role);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to create open role due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.OpenRoles.Add(role);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to create open role {role.RoleTitle}");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> UpdateOpenRoleAsync(OpenRole role)
        {
            try
            {
                ValidationResult results = await _openRoleValidator.ValidateAsync(role);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to update open role due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        //logic to display error messages to user
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.OpenRoles.Update(role);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to update open role {role.RoleTitle}");
                throw;
            }
        }

        public async Task<int> SoftDeleteOpenRoleAsync(int roleId)
        {
            OpenRole? openRole = new();
            try
            {
                openRole = await _unitOfWork.OpenRoles.GetOpenRoleByIdAsync(roleId);
                _unitOfWork.OpenRoles.SoftDelete(openRole!);
                return await _unitOfWork.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to soft delete open role {openRole!.RoleTitle}");
                throw;
            }
        }
    }
}
