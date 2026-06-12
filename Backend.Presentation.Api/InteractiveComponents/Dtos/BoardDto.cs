namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

/// <summary>
/// Data Transfer Object (DTO) representing a <see cref="Board"/> for API responses or requests.
/// </summary>
/// <remarks>
/// This DTO is used to transfer board data between the API layer and clients without exposing
/// the internal domain model directly. It includes all relevant attributes needed for
/// display, creation, or update operations in the API.
/// </remarks>
/// <param name="Color">The primary color of the board.</param>
/// <param name="MarkerColor">The color of the markers associated with the board.</param>
/// <param name="Texture">The texture applied to the board surface.</param>
/// <param name="PlateId">Unique identifier assigned to the board.</param>
/// <param name="X">The X-coordinate of the board in the park environment.</param>
/// <param name="Y">The Y-coordinate of the board in the park environment.</param>
/// <param name="Z">The Z-coordinate (height) of the board in the park environment.</param>
/// <param name="Width">The width of the board.</param>
/// <param name="Height">The height of the board.</param>
/// <param name="Depth">The depth of the board.</param>
/// <param name="XAxisRotation">The rotation of the board around the X-axis.</param>
/// <param name="YAxisRotation">The rotation of the board around the Y-axis.</param>
/// <param name="ZAxisRotation">The rotation of the board around the Z-axis.</param>
/// <param name="LearningSpaceId">The ID of the learning space that this board belongs to.</param>
public record BoardDto(
    string Color,
    string MarkerColor,
    string Texture,
    string PlateId,
    double X,
    double Y,
    double Z,
    double Width,
    double Height,
    double Depth,
    double XAxisRotation,
    double YAxisRotation,
    double ZAxisRotation,
    int LearningSpaceId
);
