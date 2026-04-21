using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.EFCore
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<User> Users { get; set; }

        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurements");

                entity.HasIndex(e => e.Operation)
                      .HasDatabaseName("idx_operation");

                entity.HasIndex(e => e.ThisMeasurementType)
                      .HasDatabaseName("idx_measurement_type");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("idx_created_at");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("NOW()");
            });
        }
        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries<QuantityMeasurementEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
