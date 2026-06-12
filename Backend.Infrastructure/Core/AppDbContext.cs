using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.EntityConfigurations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.EntityConfigurations;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.EntityConfigurations;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.EntityConfigurations;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Permissions.EntityConfigurations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.EntityConfiguration;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

/// <summary>
/// Represents the application's database context.
/// </summary>
/// <remarks>  This class is used to configure and manage the database 
/// access through Entity Framework Core.
/// </remarks>
internal class AppDbContext : DbContext
{
        
    public virtual DbSet<Building> Building { get; set; } = null!;
    public virtual DbSet<BuildingRenderInfo> BuildingRenderInfo { get; set; } = null!;

    /// <summary>
    /// Maps to a database table for Users 
    /// </summary>
    public virtual DbSet<User> Users { get; set; } = null!; // set via EF

    /// <summary>
    /// Maps Role entities to a database table
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; } = null!;

    /// <summary>
    /// DbSet representing Permission entities in the database
    /// </summary>
    public virtual DbSet<Permission> Permissions { get; set; } = null!; 
        
    /// <summary>
    /// DbSet representing of LearningSpace entities in the database.
    /// </summary>
    public virtual DbSet<LearningSpace> LearningSpaces { get; set; } = null!;

    /// <summary>
    /// DbSet representing Laboratory entities in the database.
    /// </summary>
    public virtual DbSet<Laboratory> Laboratories { get; set; } = null!;

    /// <summary>
    /// DbSet representing Classroom entities in the database.
    /// </summary>
    public virtual DbSet<Classroom> Classrooms { get; set; } = null!;

    /// <summary>
    /// DbSet representing LearningSpace Textures in the database.
    /// </summary>
    public virtual DbSet<LearningSpaceTexture> LearningSpaceTextures { get; set; } = null!;

    /// <summary>
    /// DbSet for InteractiveComponents entities.
    /// </summary>
    public virtual DbSet<InteractiveComponent> InteractiveComponents { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    /// <summary>
    /// Default constructor that receives no parameters.
    /// </summary>
    [Obsolete("For use by mocking libraries only. DO NOT USE in code.")]
    public AppDbContext()
    {
    }

    /// <summary>
    /// Configures the entity model for the context.
    /// Currently, it does not apply any custom configurations.
    /// Write in this function the method to configure new entities.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InteractiveComponent>().UseTptMappingStrategy();
        modelBuilder.ApplyConfiguration(new InteractiveComponentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BoardEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectorEntityConfiguration());

        modelBuilder.ApplyConfiguration(new BuildingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BuildingRenderInfoEntityConfiguration());
        
        // Apply configurations for LearningSpace entity.
        // Apply TPT strategy for LearningSpace and its derived types.
        modelBuilder.Entity<LearningSpace>().UseTptMappingStrategy();
        modelBuilder.ApplyConfiguration(new LearningSpaceEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LaboratoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ClassroomEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LearningSpaceTextureEntityConfiguration());

        // Apply configurations for User entity.
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());


        // Apply configurations for Permission entity.
        modelBuilder.ApplyConfiguration(new PermissionEntityConfiguration());

        // Apply configurations for Role entity.
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

    }

}