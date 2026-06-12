using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles retrieving all <see cref="Projector"/> interactive components from the system.
/// </summary>
/// <remarks>
/// This handler performs the following steps:
/// 1. Validates that the provided <see cref="IInteractiveComponentService"/> is not null.
/// 2. Calls <see cref="IInteractiveComponentService.ListAllProjectorsAsync"/> to retrieve all Projectors from the system.
/// 3. Maps the domain <see cref="Projector"/> entities to data transfer objects (DTOs) using <see cref="ProjectorDtoMapper"/>.
/// 4. Wraps the list of board DTOs in a <see cref="ListAllProjectorsResponse"/> object for API response.
/// </remarks>
public static class ListAllProjectorsHandler
{
    /// <summary>
    /// Processes a request to list all Projectors.
    /// </summary>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for business logic related to interactive components.
    /// Must not be null.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ListAllProjectorsResponse}"/> containing all projectors in the system,
    /// wrapped as a <see cref="ListAllProjectorsResponse"/> object.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="interactiveComponentService"/> is null.
    /// </exception>
    public static async Task<ListAllProjectorsResponse> HandleAsync(
        IInteractiveComponentService interactiveComponentService)
    {
        if (interactiveComponentService is null)
        {
            throw new ArgumentNullException(nameof(interactiveComponentService));
        }

        var boards = await interactiveComponentService.ListAllProjectorsAsync()
            .ConfigureAwait(false);

        var projectorDtos = boards
            .Select(ProjectorDtoMapper.ToDto)
            .ToList();

        return new ListAllProjectorsResponse(projectorDtos);
    }
}
