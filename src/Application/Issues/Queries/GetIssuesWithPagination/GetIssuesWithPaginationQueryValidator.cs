using FluentValidation;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesWithPagination
{
    public class GetIssuesWithPaginationQueryValidator : AbstractValidator<GetIssuesWithPaginationQuery>
    {
        public GetIssuesWithPaginationQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty().WithMessage("ProjectId is required.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}
