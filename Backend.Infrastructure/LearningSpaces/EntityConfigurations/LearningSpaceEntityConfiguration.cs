using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.EntityConfigurations;

/// <summary>
/// Represents the EF Core configuration for the LearningSpace entity.
/// </summary>
internal class LearningSpaceEntityConfiguration : IEntityTypeConfiguration<LearningSpace>
{
    /// <summary>
    /// Sets up the EF Core configuration for the LearningSpace entity.
    /// </summary>
    /// <param name="builder">Represents the builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<LearningSpace> builder)
    {
        // Configure table name, schema.
        builder.ToTable("LearningSpace", "LearningSpaces")
            .HasKey("Id");

        // PK: Configure Id as internal primary key.
        builder.Property<int>("Id")
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        // FK: Configure BuildingId (can be null if no relation exists yet).
        builder.Property(ls => ls.BuildingId)
            .HasColumnName("BuildingId")
            .IsRequired(false);

        // FK: Configure FloorLevel (can be null if no relation exists yet).
        builder.Property(ls => ls.FloorLevel)
            .HasColumnName("FloorLevel")
            .IsRequired(false);

        // Configure RoomId (not primary key).
        builder.Property(ls => ls.RoomId)
            .HasColumnName("RoomId")
            .HasMaxLength(50)
            .IsRequired();

        // Configure color property using LearningSpaceColor value object
        builder.Property(ls => ls.Color)
            .HasColumnName("Color")
            .HasColumnType("nvarchar(10)")
            .HasConversion(
                convertToProviderExpression: color => color.Value,
                convertFromProviderExpression: colorValue => LearningSpaceColor.Create(colorValue)
            )
            .HasDefaultValue(LearningSpaceColor.Create("#CDCECF"))
            .IsRequired();

        // Configure texture property using LearningSpaceTexture value object
        builder.Property(ls => ls.Texture)
            .HasColumnName("Texture")
            .HasColumnType("nvarchar(50)")
            .HasConversion(
                convertToProviderExpression: texture => texture!.Value,
                convertFromProviderExpression: textureValue => LearningSpaceTexture.Create(textureValue)
            )
            .IsRequired();

        // Configure LearningSpaceDimensions value object.
        builder.OwnsOne(ls => ls.Dimensions, dimensions =>
            {
                dimensions.Property(d => d.Length)
                    .HasColumnName("Length")
                    .IsRequired();

                dimensions.Property(d => d.Width)
                    .HasColumnName("Width")
                    .IsRequired();

                dimensions.Property(d => d.Height)
                    .HasColumnName("Height")
                    .IsRequired();
            });

        // Configure LearningSpaceCoordinates value object.
        builder.OwnsOne(ls => ls.Coordinates, coordinates =>
            {
                coordinates.Property(c => c.XCoordinate)
                    .HasColumnName("XCoordinate")
                    .IsRequired();

                coordinates.Property(c => c.YCoordinate)
                    .HasColumnName("YCoordinate")
                    .IsRequired();

                coordinates.Property(c => c.ZCoordinate)
                    .HasColumnName("ZCoordinate")
                    .IsRequired();
            });

        // Configure unique constraint for RoomId + BuildingId.
        builder.HasIndex("RoomId", "BuildingId")
            .HasDatabaseName("UQ_Room_Building")
            .IsUnique();


        // Important: Use Restrict to prevent deletion of a Building while LearningSpaces reference it.
        // This ensures that a Building cannot be deleted if it contains any LearningSpaces.
        // Expected workflow: Before deleting a Building, all associated LearningSpaces must be removed or reassigned.
        builder.HasOne(ls => ls.Building)
            .WithMany(b => b.LearningSpaces)
            .HasForeignKey(ls => ls.BuildingId)
            .HasConstraintName("FK_LearningSpace_Building")
            .OnDelete(DeleteBehavior.Restrict);

        // When we add more columns for Laboratory, we will configure them here. 
    }
}
