using FluentValidation;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecruitmentApp.BAL.Validators
{
    public class RecruiterValidator : AbstractValidator<Recruiter>
    {
        private IUnitOfWork _unitOfWork;

        public RecruiterValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(recruiter => recruiter.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Length(2, 50).WithMessage("Length ({TotalLength}) of {PropertyName} is invalid.")
                .Must(ValidationHelper.BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(recruiter => recruiter.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Length(2, 50).WithMessage("Length ({TotalLength}) of {PropertyName} is invalid.")
                .Must(ValidationHelper.BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(role => role.Seniority)
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(ValidationHelper.BeAValidString).WithMessage("{PropertyName} contains invalid characters")
                .When(role => !string.IsNullOrWhiteSpace(role.Seniority));
        }

    }
}
