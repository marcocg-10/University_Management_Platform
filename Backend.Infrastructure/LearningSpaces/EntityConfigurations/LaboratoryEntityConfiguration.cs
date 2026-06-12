using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.EntityConfigurations;

/// <summary>
/// Represents the EF Core configuration for the Laboratory entity that inherits from
/// LearningSpace.
/// </summary>
internal class LaboratoryEntityConfiguration : IEntityTypeConfiguration<Laboratory>
{
    /// <summary>
    /// Sets up the EF Core configuration for the Laboratory entity.
    /// </summary>
    /// <param name="builder">Represents the builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Laboratory> builder)
    {
        // Configure table name, schema.
        builder.ToTable("Laboratory", "LearningSpaces");

        // Same PK as LearningSpace (FK to base class).
        builder.HasBaseType<LearningSpace>();

        // When we add more columns for Laboratory, we will configure them here. 
    }
}
