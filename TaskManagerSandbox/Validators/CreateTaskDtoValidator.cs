using FluentValidation;
using TaskManagerSandbox.DTOs;

namespace TaskManagerSandbox.Validators;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(BeValidPriority).WithMessage("Priority must be one of: Low, Medium, High, Critical");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).When(x => x.DueDate.HasValue)
            .WithMessage("Due date must be in the future");

        RuleFor(x => x.AssignedTo)
            .MaximumLength(100).WithMessage("AssignedTo cannot exceed 100 characters");

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0).When(x => x.EstimatedHours.HasValue)
            .WithMessage("Estimated hours must be greater than 0");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).When(x => x.CategoryId.HasValue)
            .WithMessage("Category ID must be greater than 0");
    }

    private bool BeValidPriority(string priority)
    {
        return priority != null && 
               (priority.Equals("Low", StringComparison.OrdinalIgnoreCase) ||
                priority.Equals("Medium", StringComparison.OrdinalIgnoreCase) ||
                priority.Equals("High", StringComparison.OrdinalIgnoreCase) ||
                priority.Equals("Critical", StringComparison.OrdinalIgnoreCase));
    }
} 