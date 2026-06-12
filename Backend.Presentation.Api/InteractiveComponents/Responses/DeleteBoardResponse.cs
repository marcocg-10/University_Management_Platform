namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by the API after successfully deleting a 
/// <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities.Board"/>.
/// </summary>
/// <param name="Message">
/// A descriptive message indicating the result of the deletion operation, typically including the board's PlateId.
/// </param>
public record DeleteBoardResponse(string Message);
