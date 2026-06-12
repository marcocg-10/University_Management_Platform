namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Responses;

/// <summary>
/// Represents the response returned after attempting to create permissions.
/// </summary>
/// <remarks>This response contains the status of the operation, indicating whether the permission creation was
/// successful or not.</remarks>
/// <param name="Status">The status of the operation. Typically, this is a string describing the outcome, such as "Success" or an error
/// message.</param>
public record CreatePermissionsResponse(String Status);
