using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.EntityConfigurations;

/// <summary>
/// Represents the EF Core configuration for the LearningSpaceTexture entity.
/// </summary>
internal class LearningSpaceTextureEntityConfiguration : IEntityTypeConfiguration<LearningSpaceTexture>
{
    /// <summary>
    /// Sets up the EF Core configuration for the LearningSpaceTexture entity.
    /// </summary>
    /// <param name="builder">Represents the builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<LearningSpaceTexture> builder)
    {
        // Configure table name, schema.
        builder.ToTable("LearningSpaceTexture", "LearningSpaces");

        builder.HasKey(lst => lst.Value);

        builder.Property(lst => lst.Value)
            .HasColumnName("Texture")
            .HasColumnType("nvarchar(50)");

    }
}
