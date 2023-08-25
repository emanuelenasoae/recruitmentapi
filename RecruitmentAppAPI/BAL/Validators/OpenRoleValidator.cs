using FluentValidation;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecruitmentApp.BAL.Validators
{
    public class OpenRoleValidator : AbstractValidator<OpenRole>
    {
        private IUnitOfWork _unitOfWork;

        public OpenRoleValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(role => role.RoleTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(ValidationHelper.BeAValidString).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(role => role.Seniority)
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(ValidationHelper.BeAValidString).WithMessage("{PropertyName} contains invalid characters")
                .When(role => !string.IsNullOrWhiteSpace(role.Seniority));

            RuleFor(role => role.SalaryRange)
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .When(role => !string.IsNullOrWhiteSpace(role.SalaryRange));

            RuleFor(role => role.StartDate)
                .NotNull().WithMessage("{PropertyName} cannot be empty");

            RuleFor(role => role.EndDate)
                .Must((role, endDate) => role.EndDate >= role.StartDate)
                .When(role => role.EndDate.HasValue);

        }
    }
}
