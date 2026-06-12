namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

/// <summary>
/// Data transfer object for building render information.
/// </summary>
/// <param name="Color"></param>
/// <param name="Height"></param>
/// <param name="Width"></param>
/// <param name="Depth"></param>
/// <param name="XCoordinate"></param>
/// <param name="YCoordinate"></param>
/// <param name="ZCoordinate"></param>
/// <param name="Texture"></param>
public record BuildingRenderInfoDto(
    string Color,
    decimal Height,
    decimal Width,
    decimal Depth,
    decimal XCoordinate,
    decimal YCoordinate,
    decimal ZCoordinate,
    string Texture
);