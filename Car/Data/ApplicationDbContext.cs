using Microsoft.EntityFrameworkCore;
using Car.Models;

namespace Car.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Branch> Branches { get; set; }
        public DbSet<GasCar> GasCars { get; set; }
        public DbSet<ElectricCar> ElectricCars { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Branch
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Branch");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Country).HasColumnName("country").IsRequired();
            });
            
            // Configure GasCar
            modelBuilder.Entity<GasCar>(entity =>
            {
                entity.ToTable("GasCar");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FuelEff).HasColumnName("fuel_eff");
                entity.Property(e => e.Model).HasColumnName("model").IsRequired();
                entity.Property(e => e.BranchId).HasColumnName("branch_id");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
                entity.Property(e => e.ImageUrl).HasColumnName("ImageUrl");
                
                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.GasCars)
                    .HasForeignKey(d => d.BranchId);
            });
            
            // Configure ElectricCar
            modelBuilder.Entity<ElectricCar>(entity =>
            {
                entity.ToTable("ElectricCar");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Battery).HasColumnName("battery");
                entity.Property(e => e.RangeKm).HasColumnName("range_km");
                entity.Property(e => e.Model).HasColumnName("model").IsRequired();
                entity.Property(e => e.BranchId).HasColumnName("branch_id");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
                entity.Property(e => e.ImageUrl).HasColumnName("ImageUrl");
                
                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.ElectricCars)
                    .HasForeignKey(d => d.BranchId);
            });
        }
    }
}
