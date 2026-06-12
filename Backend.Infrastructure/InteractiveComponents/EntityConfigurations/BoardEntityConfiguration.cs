using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.EntityConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Board"/> entity.
/// </summary>
/// <remarks>
/// Configures database-specific mappings and constraints for the <see cref="Board"/> entity,
/// including value object conversions, column names, length restrictions, and nullability.
/// This class ensures that the <see cref="Board"/> entity is properly persisted in the database
/// while maintaining the domain model integrity.
/// </remarks>
internal class BoardEntityConfiguration : IEntityTypeConfiguration<Board>
{
    /// <summary>
    /// Configures the <see cref="Board"/> entity using the provided <see cref="EntityTypeBuilder{Board}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Board"/> entity.</param>
    /// <remarks>
    /// This configuration specifically handles:
    /// <list type="bullet">
    /// <item>
    /// <description>Mapping the <see cref="Board.MarkerColor"/> value object to a string column in the database.</description>
    /// </item>
    /// <item>
    /// <description>Setting a maximum length of 50 characters for the MarkerColor column.</description>
    /// </item>
    /// <item>
    /// <description>Renaming the column in the database to "MarkerColor".</description>
    /// </item>
    /// </list>
    /// </remarks>
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Board", "InteractiveComponents");
        builder.Property(b => b.MarkerColor)
            .HasConversion(
                color => color!.Value,      // Convert Color value object to string for storage
                value => new Color(value))  // Convert string from database back to Color value object
            .HasMaxLength(50)               // Set maximum column length to 50 characters
            .HasColumnName("MarkerColor")  // Set explicit column name
            .IsRequired();                  // MarkerColor is required
    }
}
