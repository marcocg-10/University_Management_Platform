using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API when listing all interactive boards.
/// </summary>
/// <param name="boards">
/// A collection of <see cref="BoardDto"/> objects representing the boards available in the system.
/// </param>
public record ListAllBoardsResponse(IEnumerable<BoardDto> boards);
