using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Requests;

/// <summary>
/// Represents the payload for creating a new <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities.Projector"/> 
/// via the API.
/// </summary>
public record CreateProjectorRequest(ProjectorDto Projector);

