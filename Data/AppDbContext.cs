using ComputerComponentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerComponentsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pc> Pcs => Set<Pc>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<PcComponent> PcComponents => Set<PcComponent>();
    public DbSet<ComponentType> ComponentTypes => Set<ComponentType>();
    public DbSet<ComponentManufacturer> ComponentManufacturers => Set<ComponentManufacturer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Warranty).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Stock).IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.ComponentManufacturer)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentManufacturerId);

            entity.HasOne(e => e.ComponentType)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentTypeId);
        });

        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.HasKey(e => new { e.PcId, e.ComponentCode });

            entity.Property(e => e.ComponentCode).HasMaxLength(10);
            entity.Property(e => e.Amount).IsRequired();

            entity.HasOne(e => e.Pc)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.PcId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.ComponentCode);
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FoundationDate).HasColumnType("date");
        });

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "INT", FullName = "Intel Corporation", FoundationDate = new DateTime(1968, 7, 18) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NVD", FullName = "NVIDIA Corporation", FoundationDate = new DateTime(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "CRS", FullName = "Corsair Gaming Inc.", FoundationDate = new DateTime(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU001", Name = "Intel Core i7", Description = "High performance CPU", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "GPU001", Name = "NVIDIA RTX 4070", Description = "Gaming graphics card", ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "RAM001", Name = "Corsair 32GB DDR5", Description = "Fast memory module", ComponentManufacturerId = 3, ComponentTypeId = 3 }
        );

        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X", Weight = 12.5m, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2m, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new Pc { Id = 3, Name = "Creator Workstation", Weight = 10.8m, Warranty = 48, CreatedAt = new DateTime(2026, 3, 20, 10, 15, 0), Stock = 3 }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PcId = 1, ComponentCode = "CPU001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "GPU001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "RAM001", Amount = 2 },
            new PcComponent { PcId = 2, ComponentCode = "CPU001", Amount = 1 },
            new PcComponent { PcId = 3, ComponentCode = "GPU001", Amount = 2 }
        );
    }
}