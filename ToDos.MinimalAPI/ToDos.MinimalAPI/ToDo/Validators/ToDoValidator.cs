using FluentValidation;

namespace ToDos.MinimalAPI.ToDo.Validators
{
    public class ToDoValidator : AbstractValidator<ToDo>
    {
        public ToDoValidator()
        {
            RuleFor(toDo => toDo.Value)
                .NotEmpty().WithMessage("The ToDo value must not be empty.")
                .MinimumLength(5).WithMessage("Value of a todo must be at least 5 characters");
        }
    }
}
