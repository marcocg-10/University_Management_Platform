using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Permissions.EntityConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Permission"/> entity.
/// </summary>
internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Configures the EF Core metadata for the <see cref="Permission"/> entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions", schema: "Users");

        builder
            .Property(permission => permission.Id)
            .HasColumnName("IDPermission")
            .ValueGeneratedOnAdd();

        builder
            .HasKey(permission => permission.Id);

        builder.Property(permission => permission.Name)
          .HasMaxLength(30)
          .IsRequired()
          .HasConversion(
              convertToProviderExpression: name => name.Value,  // name to string
              convertFromProviderExpression: nameString => PermissionName.Create(nameString)); // string to permmission name VO

        builder
            .HasIndex(permission => permission.Name)
            .IsUnique();
    }
}
