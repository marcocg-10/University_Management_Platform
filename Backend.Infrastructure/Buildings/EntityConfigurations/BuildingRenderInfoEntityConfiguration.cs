using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.EntityConfigurations;

/// <summary>
/// Configures the EF Core mapping for <see cref="BuildingRenderInfo"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines how the <see cref="BuildingRenderInfo"/> entity is
/// mapped to the database using Entity Framework Core.  
/// It sets up table mapping, primary key, property constraints, and value object conversions.
/// </remarks>
internal class BuildingRenderInfoEntityConfiguration : IEntityTypeConfiguration<BuildingRenderInfo>
{
    /// <summary>
    /// Configures the <see cref="BuildingRenderInfo"/> entity's database mapping, property conversions, constraints, and value object relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<BuildingRenderInfo> builder)
    {
        // Configure table name
        builder.ToTable("BuildingRenderInfo", "buildings")
            .HasKey("Id");
            
        // Configure color property
        builder.Property(BuildingRenderInfo => BuildingRenderInfo.Color)
            .HasColumnName("Color")
            .HasColumnType("nvarchar(10)")
            .HasConversion(
                 convertToProviderExpression: BuildingRenderInfo => BuildingRenderInfo.Value,
                 convertFromProviderExpression: BuildingColorStr => Color.Create(BuildingColorStr)
            )
            .HasDefaultValue(Color.Create("#CDCECF"))
            .IsRequired();

        // Configure official id, using the OfficialId property
        builder.Property(BuildingRenderInfo => BuildingRenderInfo.BuildingId)
            // Mark the OfficialId property as required
            .IsRequired()
            // Define the maximum length for the OfficialId property
            .HasMaxLength(30);

        // Configure Dimensions value object Heigth
        builder.Property(building => building.Heigth)
            .HasColumnName("Height")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.Value,
                 convertFromProviderExpression: BuildingHeigthStr => Heigth.Create(BuildingHeigthStr)
            )
            // Mark the Name property as required
            .IsRequired();

        // Configure Dimensions value object Width
        builder.Property(building => building.Width)
            .HasColumnName("Width")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.Value,
                 convertFromProviderExpression: BuildingWidthStr => Width.Create(BuildingWidthStr)
            )
            // Mark the Name property as required
            .IsRequired();


        // Configure Dimensions value object Depth
        builder.Property(building => building.Depth)
            .HasColumnName("Depth")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.Value,
                 convertFromProviderExpression: BuildingDepthStr => Depth.Create(BuildingDepthStr)
            )
            // Mark the Name property as required
            .IsRequired();


        // Configure Dimensions value object XCoodinate
        builder.Property(building => building.XCoodinate)
            .HasColumnName("X")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.XValue,
                 convertFromProviderExpression: BuildingXStr => X.Create(BuildingXStr)
            )
            // Mark the Name property as required
            .IsRequired();

        // Configure Dimensions value object YCoodinate
        builder.Property(building => building.YCoodinate)
            .HasColumnName("Y")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.YValue,
                 convertFromProviderExpression: BuildingYStr => Y.Create(BuildingYStr)
            )
            // Mark the Name property as required
            .IsRequired();

        // Configure Texture value object
        builder.Property(building => building.Texture)
            .HasColumnName("Texture")
            .HasColumnType("nvarchar(50)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.Value,
                 convertFromProviderExpression: BuildingTextureStr => BuildingTexture.Create(BuildingTextureStr)
            )
            // Mark the Texture property as required
            .IsRequired();

        // Configure Dimensions value object ZCoodinate
        builder.Property(building => building.ZCoodinate)
            .HasColumnName("Z")
            .HasColumnType("decimal(18,2)")
            .HasConversion(
                 convertToProviderExpression: Building => Building.ZValue,
                 convertFromProviderExpression: BuildingZStr => Z.Create(BuildingZStr)
            )
            // Mark the Name property as required
            .IsRequired();

    }
}

