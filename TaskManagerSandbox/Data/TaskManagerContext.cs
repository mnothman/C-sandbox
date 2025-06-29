using Microsoft.EntityFrameworkCore;
using TaskManagerSandbox.Models;

namespace TaskManagerSandbox.Data;

public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TaskItem entity
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            // Configure Tags as JSON
            entity.Property(e => e.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
                );

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Bio).HasMaxLength(500);

            // Unique constraints
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Color).HasMaxLength(7);

            // Relationships
            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.Categories)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Users
        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@taskmanager.com",
            FirstName = "Admin",
            LastName = "User",
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var regularUser = new User
        {
            Id = 2,
            Username = "user",
            Email = "user@taskmanager.com",
            FirstName = "Regular",
            LastName = "User",
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Seed Categories
        var workCategory = new Category
        {
            Id = 1,
            Name = "Work",
            Description = "Work-related tasks",
            Color = "#007bff",
            CreatedById = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var personalCategory = new Category
        {
            Id = 2,
            Name = "Personal",
            Description = "Personal tasks",
            Color = "#28a745",
            CreatedById = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var urgentCategory = new Category
        {
            Id = 3,
            Name = "Urgent",
            Description = "Urgent tasks that need immediate attention",
            Color = "#dc3545",
            CreatedById = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        modelBuilder.Entity<User>().HasData(adminUser, regularUser);
        modelBuilder.Entity<Category>().HasData(workCategory, personalCategory, urgentCategory);
    }
} 