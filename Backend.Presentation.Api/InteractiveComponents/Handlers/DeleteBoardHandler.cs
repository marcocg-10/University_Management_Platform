using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles the deletion of a <see cref="Board"/> interactive component via the API.
/// </summary>
public static class DeleteBoardHandler
{
    /// <summary>
    /// Processes a request to delete a board by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">
    /// The unique identifier (PlateId) of the board to delete.
    /// Must correspond to an existing board in the system.
    /// </param>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for business logic related to interactive components.
    /// </param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// HTTP 200 OK with <see cref="DeleteBoardResponse"/> on successful deletion.
    /// </returns>
    public static async Task<IResult> HandleAsync(
        string plateId,
        IInteractiveComponentService interactiveComponentService)
    {
        await interactiveComponentService.DeleteBoardAsync(plateId);

        var response = new DeleteBoardResponse($"The board with ID: {plateId} was deleted successfully.");

        return TypedResults.Ok(response);
    }
}
