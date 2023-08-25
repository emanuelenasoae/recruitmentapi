using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.BAL.Validators;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.ServicesImplementation
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RecruiterValidator _recruiterValidator;
        private readonly ILogger _logger;

        public RecruiterService(IUnitOfWork unitOfWork,
            RecruiterValidator recruiterValidator,
            ILogger<RecruiterService> logger)
        {
            _unitOfWork = unitOfWork;
            _recruiterValidator = recruiterValidator;
            _logger = logger;
        }

        public async Task<Recruiter?> GetRecruiterByIdAsync(int recruiterId)
        {
            try
            {
                return await _unitOfWork.Recruiters.GetRecruiterByIdAsync(recruiterId);
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch recruiter with id {recruiterId}");
                throw;
            }
        }

        public async Task<List<Recruiter>> GetRecruitersByFirstName(string firstName)
        {
            try
            {
                return await _unitOfWork.Recruiters.GetByFirstNameAsync(firstName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch recruiters with first name {firstName}");
                throw;
            }
        }

        public async Task<IEnumerable<Recruiter?>> GetAllRecruitersAsync()
        {
            try
            {
                return await _unitOfWork.Recruiters.GetAllRecruitersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch all recruiters");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> CreateRecruiterAsync(Recruiter recruiter)
        {
            try
            {
                ValidationResult results = await _recruiterValidator.ValidateAsync(recruiter);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to create recruiter due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.Recruiters.Add(recruiter);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to create recruiter {recruiter.FirstName}");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> UpdateRecruiterAsync(Recruiter recruiter)
        {
            try
            {
                ValidationResult results = await _recruiterValidator.ValidateAsync(recruiter);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to update recruiter due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        //logic to display error messages to user
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.Recruiters.Update(recruiter);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to update recruiter {recruiter.FirstName}");
                throw;
            }
        }
    }
}
