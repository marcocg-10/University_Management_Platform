using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.EntityConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// Maps the domain entity to the relational model while keeping domain purity:
/// - Table: User
/// - Primary Key: IdKey (internal surrogate key)
/// - Public natural identifier: Id (column: IdUser, fixed max length constraint)
/// - Value Object: Email stored as its underlying string via a value converter
/// - Required fields: Id, Name, Email, IsActive
/// - Optional field: AzureObjectIdentifier for Azure AD integration
/// </remarks>
internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the EF Core metadata for the <see cref="User"/> entity:
    /// 1. Sets table name and primary key.
    /// 2. Maps the natural identifier (<see cref="User.Id"/>) to column IdUser with length constraint.
    /// 3. Applies required + length constraints to string fields.
    /// 4. Persists the <see cref="Email"/> value object using a conversion (VO &lt;-&gt; string).
    /// 5. Configures the Azure Object Identifier as an optional unique property.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", schema: "Users");

        builder
            .Property<int>("IdKey")
            .HasColumnName("IdKey")
            .ValueGeneratedOnAdd();

        builder
            .HasKey("IdKey");

        builder.Property(user => user.Id)
            .HasColumnName("IdUser")
            .HasMaxLength(30)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: idUser => idUser.Value, // string to string
                convertFromProviderExpression: idUserString => UserId.Create(idUserString)); // string to string

        builder.Property(user => user.Name)
            .HasColumnName("Name")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: name => name.Value, // string to string
                convertFromProviderExpression: nameString => UserName.Create(nameString)); // string to string

        builder.Property(user => user.IsActive)
            .HasColumnName("IsActive")
            .IsRequired();

        builder.Property(student => student.Email)
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: email => email.Value,  // email to string
                convertFromProviderExpression: emailString => Email.Create(emailString)); // string to email VO

        builder.Property(user => user.AzureObjectIdentifier)
            .HasColumnName("AzureObjectIdentifier")
            .HasMaxLength(36)
            .IsRequired(false);

        builder.Property(student => student.AvatarId)
            .HasColumnName("AvatarId")
            .HasMaxLength(50)
            .IsRequired(false)
            .HasConversion(
                convertToProviderExpression: avatarId => avatarId != null ? avatarId.Value : null,  // AvatarId to string
                convertFromProviderExpression: avatarIdString => avatarIdString != null ? AvatarId.Create(avatarIdString) : null); // string to AvatarId VO

        builder.HasIndex(user => user.AzureObjectIdentifier)
            .IsUnique()
            .HasFilter("[AzureObjectIdentifier] IS NOT NULL"); // For faster traversing.
        // Many-to-many relationship between Role and Permission
        builder
         .HasMany(e => e.Roles)
         .WithMany();
    }
}
