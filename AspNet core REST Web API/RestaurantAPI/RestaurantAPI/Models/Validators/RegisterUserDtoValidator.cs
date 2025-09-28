using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext) 
        {
            Console.WriteLine("Trying to validate the user register request");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

            RuleFor(x => x.Email)
                .Custom((value, context) => 
                {
                    bool emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email is already in use");
                    }
                });
        }
    }
}
