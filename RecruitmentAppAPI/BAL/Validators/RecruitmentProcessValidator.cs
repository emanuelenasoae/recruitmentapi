using FluentValidation;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.Validators
{
    public class RecruitmentProcessValidator : AbstractValidator<RecruitmentProcess>
    {
        private IUnitOfWork _unitOfWork;

        public RecruitmentProcessValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(process => process.CandidateId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is not valid")
                .MustAsync(BeAnExistingCandidate).WithMessage("Recruitment Process must be assigned to an existing {PropertyName}");

            RuleFor(process => process.RecruiterId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is not valid")
                .MustAsync(BeAnExistingRecruiter).WithMessage("Recruitment Process must be assigned to an existing {PropertyName}");

            RuleFor(process => process.Stage)
                .Cascade(CascadeMode.Stop)
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(ValidationHelper.BeAValidString).WithMessage("{PropertyName} contains invalid characters")
                .When(process => !string.IsNullOrWhiteSpace(process.Stage));
        }
        private async Task<bool> BeAnExistingCandidate(int candidateId, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Candidates.GetCandidateByIdAsync(candidateId);
            return candidate != null ? true : false;
        }

        private async Task<bool> BeAnExistingRecruiter(int recruiterId, CancellationToken cancellationToken)
        {
            var recruiter = await _unitOfWork.Recruiters.GetRecruiterByIdAsync(recruiterId);
            return recruiter != null ? true : false;
        }
    }
}
