using FluentValidation;
using GlobusAssessment.Application.DTOs;

namespace GLobusAssessment.Api.Validators
{
    public class ConfirmPhoneNumberModelValidator : AbstractValidator<ConfirmPhoneNumberDto>
    {
        public ConfirmPhoneNumberModelValidator()
        {
            RuleFor(q => q.OTP).NotEmpty().NotNull().WithMessage("OTP is required")
                .MaximumLength(6).WithMessage("OPT cannot be more than 6 characters.")
                .MinimumLength(6).WithMessage("OPT cannot be less than 6 characters.")
                .Matches(@"^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$").WithMessage("OPT can only contain numerical values");
        }
    }
}
