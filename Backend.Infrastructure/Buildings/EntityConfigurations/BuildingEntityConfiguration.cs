using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.EntityConfigurations;

/// <summary>
/// Configures the EF Core mapping for <see cref="Building"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines how the <see cref="Building"/> entity is
/// mapped to the database using Entity Framework Core.  
/// It sets up table mapping, primary key, property constraints, and relationships.
/// </remarks>
internal class BuildingEntityConfiguration : IEntityTypeConfiguration<Building>
{
    /// <summary>
    /// Configures the <see cref="Building"/> entity's database mapping, property conversions, constraints, and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        // Configure table name
        builder.ToTable("Building", "buildings")
            .HasKey("Id");

        // Configure official id, using the OfficialId property
        builder.Property(building => building.OfficialId)
            .HasConversion(
                convertToProviderExpression: OfficialId => OfficialId.Value,
                convertFromProviderExpression: OfficialIdStr => BuildingOfficialId.Create(OfficialIdStr)
            )
            // Mark the OfficialId property as required
            .IsRequired()
            // Define the maximum length for the OfficialId property
            .HasMaxLength(30);

        // Configure Name property with conversion, required constraint, and max length
        builder.Property(building => building.Name)
            .HasConversion(
                convertToProviderExpression: Building => Building.Value,
                convertFromProviderExpression: BuildingNameStr => BuildingName.Create(BuildingNameStr)
            )
            // Mark the Name property as required
            .IsRequired()
            // Define the maximum length for the Name property
            .HasMaxLength(200);

        // Configure FloorCount property with conversion and required constraint
        builder.Property(building => building.FloorCount)
            .HasConversion(
                convertToProviderExpression: FloorCount => FloorCount.Value,
                convertFromProviderExpression: FloorCountInt => FloorCount.Create(FloorCountInt)
            )
            // Mark the FloorCount property as required
            .IsRequired();

        builder.HasOne(building => building.RenderInfo)
            .WithOne(renderInfo => renderInfo.Building)
            .HasForeignKey<BuildingRenderInfo>(buildingRenderInfo => buildingRenderInfo.BuildingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
