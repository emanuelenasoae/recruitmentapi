using RecruitmentApp.BAL.Validators;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.IRepositories;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using RecruitmentApp.BAL.ServicesAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace RecruitmentApp.BAL.ServicesImplementation
{
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CandidateValidator _candidateValidator;
        private readonly ILogger<CandidateService> _logger;

        public CandidateService(IUnitOfWork unitOfWork,
            CandidateValidator candidateValidator,
            ILogger<CandidateService> logger)
        {
            _unitOfWork = unitOfWork;
            _candidateValidator = candidateValidator;
            _logger = logger;
        }

        public async Task<Candidate?> GetCandidateByIdAsync(int candidateId)
        {
            try
            {
                return await _unitOfWork.Candidates.GetCandidateByIdAsync(candidateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to fetch candidate with id {candidateId}");
                throw;
            }
        }

        public async Task<IEnumerable<Candidate?>> GetAllCandidatesAsync()
        {
            try
            {
                return await _unitOfWork.Candidates.GetAllCandidatesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while trying to fetch all candidates");
                throw;
            }
        }

        public async Task<List<ValidationFailure>> CreateCandidateAsync(Candidate candidate)
        {
            try
            {
                ValidationResult results = await _candidateValidator.ValidateAsync(candidate);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to create candidate due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        _logger.LogInformation(failure.ErrorMessage); //failure.PropertyName to return what property is wrong
                    }
                }
                else
                {
                    _unitOfWork.Candidates.Add(candidate);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating the candidate {candidate.FirstName}");
                throw;
            }

        }

        public async Task<List<ValidationFailure>> UpdateCandidateAsync(Candidate candidate)
        {
            try
            {
                ValidationResult results = await _candidateValidator.ValidateAsync(candidate);
                if (!results.IsValid)
                {
                    _logger.LogInformation("Unable to update candidate due to following validation errors:");
                    foreach (var failure in results.Errors)
                    {
                        //logic to display error messages to user
                        _logger.LogInformation(failure.ErrorMessage); //failure.PropertyName to return what property is wrong
                    }
                }
                else
                {
                    _unitOfWork.Candidates.Update(candidate);
                    await _unitOfWork.SaveChangesAsync();
                }
                return results.Errors;
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating the candidate {candidate.FirstName}");
                throw;
            }

        }

        public async Task<int> SoftDeleteCandidateAsync(int candidateId)
        {
            Candidate? candidate = new();
            try
            {
                candidate = await _unitOfWork.Candidates.GetCandidateByIdAsync(candidateId);
                _unitOfWork.Candidates.SoftDelete(candidate!);
                return await _unitOfWork.SaveChangesAsync();
            } catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred while trying to soft delete candidate {candidate!.FirstName}");
                throw;
            }

        }
    }
}