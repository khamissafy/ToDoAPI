using FluentValidation;
using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;

namespace ToDoListAPI.Models.ToDoListManagement.Validations
{
    public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
    {
        public CreateTodoDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
