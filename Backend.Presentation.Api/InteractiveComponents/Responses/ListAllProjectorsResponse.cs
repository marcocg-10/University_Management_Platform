using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API when listing all interactive Projectors.
/// </summary>
/// <param name="projectors">
/// A collection of <see cref="ProjectorDto"/> objects representing the Projectors available in the system.
/// </param>
public record ListAllProjectorsResponse(IEnumerable<ProjectorDto> projectors);