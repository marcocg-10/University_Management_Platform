using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Utilities;

/// <summary>
/// Utility class for extracting and validating Azure Entra ID claims.
/// </summary>
public static class AzureClaimsExtractor
{
    /// <summary>
    /// Extracts and validates required claims based on the actual claims from Azure Entra ID
    /// </summary>
    /// <param name="user">The authenticated user's claims principal.</param>
    /// <returns>A result containing the extracted claims or error information.</returns>
    public static ClaimsExtractionResult ExtractUserClaims(ClaimsPrincipal user)
    {
        var claims = user.Claims.ToList();

        // User's Object ID
        var objectId = GetClaimValue(claims, "http://schemas.microsoft.com/identity/claims/objectidentifier");

        // Email Addresses
        var email = GetClaimValue(claims, "emails");

        // Full Name
        var fullName = GetClaimValue(claims, "extension_FullName");

        // Identification 
        var identification = GetClaimValue(claims, "extension_Identification");

        // User is new
        var isNewUserClaim = GetClaimValue(claims, "newUser");
        var isNewUser = bool.TryParse(isNewUserClaim, out var parsed) ? parsed : false;

        // Validate required claims
        var missingClaims = new List<string>();

        if (string.IsNullOrEmpty(objectId))
            missingClaims.Add("objectidentifier (User's Object ID)");

        if (string.IsNullOrEmpty(email))
            missingClaims.Add("emails (Email Addresses)");

        if (string.IsNullOrEmpty(fullName))
            missingClaims.Add("extension_FullName (Full Name)");

        if (missingClaims.Any())
        {
            var availableClaims = string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}"));
            return ClaimsExtractionResult.Failure($"Missing required claims: {string.Join(", ", missingClaims)}. Available claims: {availableClaims}");
        }

        return ClaimsExtractionResult.Success(new UserClaimsDataDto
        {
            AzureObjectIdentifier = objectId!,
            Email = email!,
            FullName = fullName!,
            Identification = identification!,
            IsNewUser = isNewUser
        });
    }

    /// <summary>
    /// Gets the value of a claim by type.
    /// </summary>
    /// <param name="claims">The collection of claims.</param>
    /// <param name="claimType">The type of claim to retrieve.</param>
    /// <returns>The claim value or null if not found.</returns>
    private static string? GetClaimValue(IEnumerable<Claim> claims, string claimType)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}

/// <summary>
/// Represents the result of claims extraction.
/// </summary>
public class ClaimsExtractionResult
{
    public bool IsSuccess { get; private set; }
    public UserClaimsDataDto? Claims { get; private set; }
    public string? ErrorMessage { get; private set; }

    private ClaimsExtractionResult() { }

    public static ClaimsExtractionResult Success(UserClaimsDataDto claims)
    {
        return new ClaimsExtractionResult
        {
            IsSuccess = true,
            Claims = claims
        };
    }

    public static ClaimsExtractionResult Failure(string errorMessage)
    {
        return new ClaimsExtractionResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
