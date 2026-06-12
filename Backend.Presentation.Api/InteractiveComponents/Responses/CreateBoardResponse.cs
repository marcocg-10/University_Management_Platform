using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API after successfully creating a <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities.Board"/>.
/// </summary>
/// <param name="Board">
/// The <see cref="BoardDto"/> representing the newly created board, including all its properties.
/// </param>
public record CreateBoardResponse(BoardDto Board);
