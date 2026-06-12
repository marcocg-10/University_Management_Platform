using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles the retrieval of a single <see cref="Board"/> by its unique PlateId.
/// </summary>
/// <remarks>
/// This handler performs the following operations:
/// 1. Invokes the <see cref="IInteractiveComponentService.ReadBoardByPlateIdAsync"/> service method
///    to fetch a board by the provided PlateId.
/// 2. If no board is found, returns an HTTP 404 Not Found response with a descriptive message.
/// 3. If a board is found, maps the domain entity to a DTO using <see cref="BoardDtoMapper"/>.
/// 4. Wraps the DTO in a <see cref="ReadBoardByPlateIdResponse"/> object and returns an HTTP 200 OK response.
/// </remarks>
public static class ReadBoardByPlateIdHandler
{
    /// <summary>
    /// Processes a request to retrieve a board by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">
    /// The unique identifier of the board to retrieve.
    /// </param>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for business logic related to interactive components.
    /// Must not be null.
    /// </param>
    /// <returns>
    /// A <see cref="Task{IResult}"/> representing the asynchronous HTTP response.
    /// The response is:
    /// - HTTP 200 OK with <see cref="ReadBoardByPlateIdResponse"/> if the board is found.
    /// </returns>
    public static async Task<IResult> HandleAsync(
        string plateId,
        IInteractiveComponentService interactiveComponentService)
    {
        var board = await interactiveComponentService
            .ReadBoardByPlateIdAsync(plateId)
            .ConfigureAwait(false);

        if (board is null)
            return Results.NotFound($"No board was found with the PlateId: {plateId}.");

        var boardDto = BoardDtoMapper.ToDto(board);
        var response = new ReadBoardByPlateIdResponse(boardDto);

        return Results.Ok(response);
    }
}
