using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.EntityConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="InteractiveComponent"/> base entity.
/// </summary>
/// <remarks>
/// Configures database-specific mappings and constraints for the <see cref="InteractiveComponent"/> entity,
/// including primary key, table mapping, value object conversions, owned types, and relationships.
/// </remarks>
internal class InteractiveComponentEntityConfiguration : IEntityTypeConfiguration<InteractiveComponent>
{
    /// <summary>
    /// Configures the <see cref="InteractiveComponent"/> entity using the provided <see cref="EntityTypeBuilder{InteractiveComponent}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    /// <remarks>
    /// This configuration covers the following aspects:
    /// <list type="bullet">
    /// <item>
    /// <description>Mapping the entity to the "InteractiveComponent" table in the "InteractiveComponents" schema.</description>
    /// </item>
    /// <item>
    /// <description>Defining a primary key "Id" with auto-generated values.</description>
    /// </item>
    /// <item>
    /// <description>Mapping value objects to database columns, including <see cref="Color"/> and <see cref="PlateId"/>.</description>
    /// </item>
    /// <item>
    /// <description>Setting string length constraints and required fields for common properties like Color, PlateId, and Texture.</description>
    /// </item>
    /// <item>
    /// <description>Configuring owned types <see cref="Coordinates"/> and <see cref="Dimensions"/> with explicit column names for X, Y, Z, Width, Height, and Depth.</description>
    /// </item>
    /// <item>
    /// <description>Defining the foreign key relationship to <see cref="LearningSpace"/> with <c>DeleteBehavior.Restrict</c>.</description>
    /// </item>
    /// <item>
    /// <description>Enforcing uniqueness of the PlateId column via an index.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public void Configure(EntityTypeBuilder<InteractiveComponent> builder)
    {
        // Table configuration
        builder.ToTable("InteractiveComponent", "InteractiveComponents");

        // Primary key
        builder.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.HasKey("Id");

        // Common properties
        builder.Property(x => x.Color)
            .HasConversion(
                color => color.Value,
                value => new Color(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PlateId)
            .HasConversion(
                plateId => plateId.Value,
                value => new PlateId(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Texture)
            .HasMaxLength(100)
            .IsRequired();

        // Owned types: Coordinates
        builder.OwnsOne(x => x.Coordinates, coord =>
            {
            coord.Property(c => c.X).HasColumnName("CoordinateX").IsRequired();
            coord.Property(c => c.Y).HasColumnName("CoordinateY").IsRequired();
            coord.Property(c => c.Z).HasColumnName("CoordinateZ").IsRequired();
        });

        // Owned types: Dimensions
        builder.OwnsOne(x => x.Dimensions, dim =>
            {
            dim.Property(d => d.Width).HasColumnName("Width").IsRequired();
            dim.Property(d => d.Height).HasColumnName("Height").IsRequired();
            dim.Property(d => d.Depth).HasColumnName("Depth").IsRequired();
        });

        // Owned types: Rotations
        builder.OwnsOne(x => x.Rotations, rot =>
            {
            rot.Property(p => p.XAxisRotation).HasColumnName("XAxisRotation").IsRequired();
            rot.Property(p => p.YAxisRotation).HasColumnName("YAxisRotation").IsRequired();
            rot.Property(p => p.ZAxisRotation).HasColumnName("ZAxisRotation").IsRequired();
        });

        // Foreign key to LearningSpace
        builder.Property(x => x.LearningSpaceId)
            .IsRequired();

        builder.HasOne(x => x.LearningSpace)
            .WithMany(ls => ls.InteractiveComponents)
            .HasForeignKey(x => x.LearningSpaceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint on PlateId
        builder.HasIndex(x => x.PlateId).IsUnique();
    }
}
