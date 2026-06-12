using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API when a board has been successfully updated.
/// </summary>
/// <param name="Board">
/// The board that was updated.
/// </param>
public record UpdateBoardResponse(BoardDto Board);
