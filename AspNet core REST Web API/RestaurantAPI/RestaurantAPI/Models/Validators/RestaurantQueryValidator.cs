using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        int[] allowedPageSizes = new[] { 5, 10, 15 };
        string[] allowedSortyByColumnNames = new[] { nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) };

        public RestaurantQueryValidator()
        {
            RuleFor(rq => rq.PageNumber).GreaterThan(0);
            RuleFor(rq => rq.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure($"Page size must be one of the following values: {string.Join(", ", allowedPageSizes)}");
                }
            });

            RuleFor(rq => rq.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortyByColumnNames.Contains(value))
                .WithMessage($"SortBy must be one of the following values: {string.Join(", ", allowedSortyByColumnNames)}");
        }
    }
}
