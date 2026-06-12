using FluentValidation;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Commands;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Validators;

public class CreateLaboratoryRequestValidator : AbstractValidator<CreateLaboratoryRequest>
{
    public CreateLaboratoryRequestValidator()
    {
        // Include rules from the base validator
        Include(new CreateLearningSpaceRequestValidator<CreateLaboratoryRequest>());
    }
}
