using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API when retrieving a single board by its PlateId.
/// </summary>
/// <param name="Board">
/// The <see cref="BoardDto"/> object containing the details of the requested board.
/// </param>
public record ReadBoardByPlateIdResponse(BoardDto Board);