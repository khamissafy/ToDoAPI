using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Helpers
{
    public class ListObjectsBase
    {
        [Required]
        public int pageIndex { get; set; }
        [Required]
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public string? OrderedBy { get; set; } = "";
    }

    public class ListObjectsBaseValidator : AbstractValidator<ListObjectsBase>
    {
        public ListObjectsBaseValidator()
        {
            RuleFor(x => x.pageIndex)
                .GreaterThanOrEqualTo(0).WithMessage("PageIndex must be greater than or equal to 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("SearchTerm must not exceed 100 characters.");
        }
    }

}
