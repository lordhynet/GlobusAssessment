using FluentValidation;
using GlobusAssessment.Application.DTOs;
using GlobusAssessment.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace GLobusAssessment.Api.Validators
{
    public class AddCustomerModelValidator : AbstractValidator<AddCustomerDto>
    {
        public AddCustomerModelValidator(ApplicationDbContext _context)
        {
            RuleFor(q => q.State).NotEmpty().NotNull().WithMessage("State is required");
            RuleFor(q => q.PhoneNumber).NotEmpty().NotNull().WithMessage("Phone number is required")
                .MaximumLength(11).WithMessage("Phone number cannot be more than 11 digits")
                .MinimumLength(11).WithMessage("Phone number cannot be less than 11 digits")
                .Matches(@"^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$").WithMessage("Phone number can only contain numerical values");
            RuleFor(q => q.Password).NotEmpty().NotNull().WithMessage("Password is required");
            RuleFor(q => q.LGA).NotEmpty().NotNull().WithMessage("LGA is required");

            When(q => !string.IsNullOrEmpty(q.LGA), () =>
            {
                RuleFor(p => p).Custom((value, context) =>
                {
                    var state = _context.States.Include(l => l.Locals).Where(x => x.Name == value.State).FirstOrDefaultAsync().Result;
                    if (!state.Locals.Any(l => l.Name == value.LGA))
                    {
                        context.AddFailure($"{value.LGA} is not a valid local government area for {value.State}.");
                    }
                });
            });

        }
    }
}
