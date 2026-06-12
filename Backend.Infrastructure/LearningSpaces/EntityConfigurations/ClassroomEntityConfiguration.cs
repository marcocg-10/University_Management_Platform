using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.EntityConfigurations;

/// <summary>
/// Represents the EF Core configuration for the Classroom entity that inherits from
/// LearningSpace.
/// </summary>
internal class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
    /// <summary>
    /// Sets up the EF Core configuration for the Classroom entity.
    /// </summary>
    /// <param name="builder">Represents the builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        // Configure table name, schema.
        builder.ToTable("Classroom", "LearningSpaces");

        // Same PK as LearningSpace (FK to base class).
        builder.HasBaseType<LearningSpace>();

        // When we add more columns for Classroom, we will configure them here. 
    }
}
