using FluentValidation;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Commands;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Validators;

public class CreateLearningSpaceRequestValidator<T> : AbstractValidator<T> where T : ICreateLearningSpaceRequest
{
    public CreateLearningSpaceRequestValidator()
    {
        RuleFor(x => x.BuildingId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A building must be selected.");

        RuleFor(x => x.FloorLevel)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A floor must be selected.");

        RuleFor(x => x.RoomId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Room ID cannot be empty.")
            .Length(2, 25)
            .WithMessage("Room ID must be between 2 and 25 characters long.");

        RuleFor(x => x.Width)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Width cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Width must be a positive number.");

        RuleFor(x => x.Length)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Length cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Length must be a positive number.");

        RuleFor(x => x.Height)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Height cannot be empty.")
                .GreaterThan(0)
                .WithMessage("Height must be a positive number.");
    }
}
