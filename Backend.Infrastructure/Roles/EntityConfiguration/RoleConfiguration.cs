using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.EntityConfiguration;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Role"/> entity.
/// </summary>
internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configures the EF Core metadata for the <see cref="Role"/> entity.
    /// </summary>
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role", schema: "Users");

        builder
            .Property(role => role.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .HasKey(role => role.Id);

        builder.Property(role => role.Name)
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
              convertToProviderExpression: name => name.Value,  // name to string
              convertFromProviderExpression: nameString => RoleName.Create(nameString)); // string to role name VO

        // Unique constraint on Name
        builder
            .HasIndex(role => role.Name)
            .IsUnique();
        // Many-to-many relationship between Role and Permission
        builder
         .HasMany(e => e.Permissions)
         .WithMany();
    }
}