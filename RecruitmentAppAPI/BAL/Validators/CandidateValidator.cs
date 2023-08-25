using FluentValidation;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
    public class CandidateValidator : AbstractValidator<Candidate>
    {
        private IUnitOfWork _unitOfWork;

        public CandidateValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(candidate => candidate.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Length(2, 50).WithMessage("Length ({TotalLength}) of {PropertyName} is invalid.")
                .Must(ValidationHelper.BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(candidate => candidate.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Length(2, 50).WithMessage("Length ({TotalLength}) of {PropertyName} is invalid.")
                .Must(ValidationHelper.BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(candidate => candidate.Email)
                .NotEmpty().WithMessage("Either {PropertyName} or PhoneNumber must be provided.")
                .When(candidate => string.IsNullOrWhiteSpace(candidate.PhoneNumber));

            RuleFor(candidate => candidate.PhoneNumber)
                .NotEmpty().WithMessage("Either {PropertyName} or Email must be provided.")
                .When(candidate => string.IsNullOrWhiteSpace(candidate.Email));
            //could validate email or phone number or both, based on data provided by the user
            RuleFor(candidate => candidate.Email)
                .Must(BeAValidEmail!).WithMessage("{PropertyValue} is not a valid {PropertyName}")
                .When(candidate => !string.IsNullOrWhiteSpace(candidate.Email));

            RuleFor(candidate => candidate.PhoneNumber)
                .Must(BeAValidPhoneNumber!).WithMessage("{PropertyValue} is not a valid {PropertyName}")
                .When(candidate => !string.IsNullOrWhiteSpace(candidate.PhoneNumber));

            RuleFor(candidate => candidate.DateOfBirth)
                .Must(BeAValidDateOfBirth).WithMessage("{PropertyValue} is not a valid {PropertyName}")
                .When(candidate => candidate.DateOfBirth.HasValue);

            RuleFor(candidate => candidate.RoleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is not valid")
                .MustAsync(BeAnExistingRole).WithMessage("Candididate must be assigned to an existing {PropertyName}");
        }

        private async Task<bool> BeAnExistingRole(int roleId, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.OpenRoles.GetOpenRoleByIdAsync(roleId);
            return role != null ? true : false;
        }   
        /// <summary>
        /// Validates email format using regex
        /// ^ — Begin the match at the start of the string
        /// [^@\s]+ — Match one or more occurrences of any character other than the @ character or whitespace
        /// @ — Match the @ character
        /// [^@\s]+ — Match one or more occurrences of any character other than the @ character or whitespace
        /// \. — Match a single period character
        /// (ro|com|net|org|gov) — Match ro or com or net or org or gov.
        /// $ — Stop matching at the end of the string.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool BeAValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(ro|com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Accepts the following formats:
        /// 123456789, 1234567890
        /// 123-456-7890, (123) 456-7890
        /// 123 456 7890, 123.456.7890
        ///+91 (123) 456-7890
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            string regex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}\d?$";
            return Regex.IsMatch(phoneNumber, regex);
        }

        private bool BeAValidDateOfBirth(DateTime? dateOfBirth)
        {
            var dif = DateTime.Now.Subtract(dateOfBirth!.Value);
            var age = dif.Days / 365;
            return age >= 18 && age <= 120 ? true : false;
        }
    }
}