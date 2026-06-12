using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles retrieving all <see cref="Board"/> interactive components from the system.
/// </summary>
/// <remarks>
/// This handler performs the following steps:
/// 1. Validates that the provided <see cref="IInteractiveComponentService"/> is not null.
/// 2. Calls <see cref="IInteractiveComponentService.ListAllBoardsAsync"/> to retrieve all boards from the system.
/// 3. Maps the domain <see cref="Board"/> entities to data transfer objects (DTOs) using <see cref="BoardDtoMapper"/>.
/// 4. Wraps the list of board DTOs in a <see cref="ListAllBoardsResponse"/> object for API response.
/// </remarks>
public static class ListAllBoardsHandler
{
    /// <summary>
    /// Processes a request to list all boards.
    /// </summary>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for business logic related to interactive components.
    /// Must not be null.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ListAllBoardsResponse}"/> containing all boards in the system,
    /// wrapped as a <see cref="ListAllBoardsResponse"/> object.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="interactiveComponentService"/> is null.
    /// </exception>
    public static async Task<ListAllBoardsResponse> HandleAsync(
        IInteractiveComponentService interactiveComponentService)
    {
        if (interactiveComponentService is null)
        {
            throw new ArgumentNullException(nameof(interactiveComponentService));
        }

        var boards = await interactiveComponentService.ListAllBoardsAsync()
            .ConfigureAwait(false);

        var boardDtos = boards
            .Select(BoardDtoMapper.ToDto)
            .ToList();

        return new ListAllBoardsResponse(boardDtos);
    }
}
