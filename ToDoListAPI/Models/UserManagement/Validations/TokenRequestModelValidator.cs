using FluentValidation;
using ToDoListAPI.Models.UserManagement.DTOs;

namespace ToDoListAPI.Models.UserManagement.Validations
{
    public class TokenRequestModelValidator : AbstractValidator<TokenRequestModel>
    {
        public TokenRequestModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.RememberMe)
                .NotNull().WithMessage("RememberMe field is required.");
        }
    }
}
