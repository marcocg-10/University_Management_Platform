namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

/// <summary>
/// Data Transfer Object (DTO) representing a <see cref="ProjectorDto"/> for API responses or requests.
/// </summary>
/// <remarks>
/// This DTO is used to transfer projector data between the API layer and clients without exposing
/// the internal domain model directly. It includes all relevant attributes needed for
/// display, creation, or update operations in the API.
/// </remarks>
/// <param name="Color">The primary color of the projector.</param>
/// <param name="Texture">The texture applied to the projector surface.</param>
/// <param name="Brightness">The brightness level of the projector.</param>
/// <param name="PlateId">Unique identifier assigned to the projector.</param>
/// <param name="ResWidth">Width of the projector's resolution.</param>
/// <param name="ResHeight">Height of the projector's resolution.</param>
/// <param name="X">The X-coordinate of the projector in the park environment.</param>
/// <param name="Y">The Y-coordinate of the projector in the park environment.</param>
/// <param name="Z">The Z-coordinate (height) of the projector in the park environment.</param>
/// <param name="Width">The width of the projector.</param>
/// <param name="Height">The height of the projector.</param>
/// <param name="Depth">The depth of the projector.</param>
/// <param name="XAxisRotation">The rotation of the projector around the X-axis.</param>
/// <param name="YAxisRotation">The rotation of the projector around the Y-axis.</param>
/// <param name="ZAxisRotation">The rotation of the projector around the Z-axis.</param>
/// <param name="LearningSpaceId">The ID of the learning space that this projector belongs to.</param>
public record ProjectorDto(
    string Color,
    string Texture,
    int Brightness,
    string PlateId,
    int ResWidth,
    int ResHeight,
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
