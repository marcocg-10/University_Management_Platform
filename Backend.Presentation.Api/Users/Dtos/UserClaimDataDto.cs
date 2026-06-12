namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos
{
    /// <summary>
    /// Represents user data extracted from Azure Entra ID claims.
    /// </summary>
    public class UserClaimsDataDto
    {
        public required string AzureObjectIdentifier { get; init; }
        public required string Email { get; init; }
        public required string FullName { get; init; }
        public required string Identification { get; init; }
        public bool IsNewUser { get; init; }
    }
}
