using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.EntityConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Projector"/> entity.
/// </summary>
/// <remarks>
/// Configures database-specific mappings and constraints for the <see cref="Projector"/> entity.
/// Includes value object conversions, column names, length restrictions, and nullability.
/// This class ensures that the <see cref="Projector"/> entity is properly persisted in the database
/// while maintaining the domain model integrity.
/// </remarks>
internal class ProjectorEntityConfiguration : IEntityTypeConfiguration<Projector>
{
    /// <summary>
    /// Configures the <see cref="Projector"/> entity using the provided <see cref="EntityTypeBuilder{Projector}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Projector"/> entity.</param>
    /// <remarks>
    /// This configuration specifically handles:
    /// <list type="bullet">
    /// <item>
    /// <description>Mapping the <see cref="Projector.Brightness"/> property to a required column in the database.</description>
    /// </item>
    /// <item>
    /// <description>Configuring the owned type <see cref="Projector.ProjectionResolution"/> with explicit column names for 
    /// Width and Height, both required.</description>
    /// </item>
    /// </list>
    /// </remarks>

    public void Configure(EntityTypeBuilder<Projector> builder)
    {
        builder.ToTable("Projector", "InteractiveComponents");

        // Brightness is specific to Projector and required
        builder.Property(x => x.Brightness)
            .HasColumnName("Brightness")
            .IsRequired();

        // Resolution is specific to Projector and required
        builder.OwnsOne(x => x.ProjectionResolution, res =>
            {
            res.Property(r => r.Width).HasColumnName("ResolutionWidth").IsRequired();
            res.Property(r => r.Height).HasColumnName("ResolutionHeight").IsRequired();
        });
    }
}
