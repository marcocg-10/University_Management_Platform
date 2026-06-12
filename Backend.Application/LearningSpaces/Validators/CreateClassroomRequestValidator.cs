using FluentValidation;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Commands;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Validators;

public class CreateClassroomRequestValidator : AbstractValidator<CreateClassroomRequest>
{
    public CreateClassroomRequestValidator()
    {
        // Include rules from the base validator
        Include(new CreateLearningSpaceRequestValidator<CreateClassroomRequest>());
    }
}
