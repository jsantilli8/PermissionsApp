using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Infrastructure.Persistence;

public class PermissionsDbContext : DbContext

{
    public PermissionsDbContext(DbContextOptions<PermissionsDbContext> options) : base(options) { }

    public DbSet<Permission>? Permissions { get; set; }
    public DbSet<PermissionType>? PermissionTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>()
            .HasOne(p => p.PermissionType)
            .WithMany(pt => pt.Permissions)
            .HasForeignKey(p => p.PermissionTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);

        // Seeder for  PermissionTypes
        modelBuilder.Entity<PermissionType>().HasData(
            new PermissionType { Id = 1, Description = "Read" },
            new PermissionType { Id = 2, Description = "Write" },
            new PermissionType { Id = 3, Description = "Execute" }
        );
    }
}
