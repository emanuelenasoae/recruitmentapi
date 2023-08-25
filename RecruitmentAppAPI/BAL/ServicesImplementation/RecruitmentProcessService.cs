using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
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
    public  class RecruitmentProcessService : IRecruitmentProcessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RecruitmentProcessValidator _validator;
        private readonly ILogger _logger;

        public RecruitmentProcessService(IUnitOfWork unitOfWork,
            RecruitmentProcessValidator recruitmentProcessValidator,
            ILogger<RecruitmentProcessService> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = recruitmentProcessValidator;
            _logger = logger;
        }

        public async Task<RecruitmentProcess?> GetRecruitmentProcessById(int processId)
        {
            try
            {
                return await _unitOfWork.RecruitmentProcesses.GetRecruitmentProcessByIdAsync(processId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch recruitment process with id {processId}");
                throw;
            }
        }

        public async Task<List<RecruitmentProcess>> GetRecruitmentProcessesByCandidateId(int candidateId)
        {
            try
            {
                return await _unitOfWork.RecruitmentProcesses.FindByAsync(candidate => candidate.CandidateId == candidateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch recruitment processes for candidate with id {candidateId}");
                throw;
            }
        }

        public async Task<List<RecruitmentProcess>> GetRecruitmentProcessesByRecruiterId(int recruiterId)
        {
            try
            {
                return await _unitOfWork.RecruitmentProcesses.FindByAsync(recruiter => recruiter.RecruiterId == recruiterId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch recruitment processes for recruiter with id {recruiterId}");
                throw;
            }
        }

        public async Task<IEnumerable<RecruitmentProcess?>> GetAllRecruitmentProcessesAsync()
        {
            try
            {
                return await _unitOfWork.RecruitmentProcesses.GetAllRecruitmentProcessesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch all recruitment processes");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> CreateRecruitmentProcessAsync(RecruitmentProcess process)
        {
            try
            {
                ValidationResult results = await _validator.ValidateAsync(process);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to create recruitment process due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.RecruitmentProcesses.Add(process);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to create recruitment process");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> UpdateRecruitmentProcessAsync(RecruitmentProcess process)
        {
            try
            {
                ValidationResult results = await _validator.ValidateAsync(process);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to update recruitment process due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        //logic to display error messages to user
                        _logger.LogInformation(failure.ErrorMessage);
                    }
                }
                else
                {
                    _unitOfWork.RecruitmentProcesses.Update(process);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to update recruitment process");
                throw;
            }
        }

        public async Task<int> DeleteRecruitmentProcessAsync(int processId)
        {
            try
            {
                var recruitmentProcess = await _unitOfWork.RecruitmentProcesses.GetRecruitmentProcessByIdAsync(processId);
                _unitOfWork.RecruitmentProcesses.Remove(recruitmentProcess!);
                return await _unitOfWork.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to hard delete recruitment process");
                throw;
            }
        }
    }
}
