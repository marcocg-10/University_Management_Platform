using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API after successfully creating a <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities.Projector"/>.
/// </summary>
/// <param name="Projector">
/// The projector that was created, represented as a <see cref="ProjectorDto"/>.
/// </param>
public record CreateProjectorResponse(ProjectorDto Projector);
