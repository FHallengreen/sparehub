using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class SpareHubDbContext(DbContextOptions<SpareHubDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("order");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.OrderNumber)
                .HasMaxLength(45)
                .HasColumnName("order_number")
                .IsRequired();

            entity.Property(e => e.SupplierOrderNumber)
                .HasMaxLength(45)
                .HasColumnName("supplier_order_number");

            entity.Property(e => e.SupplierId)
                .HasColumnName("supplier_id");

            entity.Property(e => e.VesselId)
                .HasColumnName("vessel_id");

            entity.Property(e => e.WarehouseId)
                .HasColumnName("warehouse_id");

            entity.Property(e => e.ExpectedReadiness)
                .HasColumnName("expected_readiness");

            entity.Property(e => e.ActualReadiness)
                .HasColumnName("actual_readiness");

            entity.Property(e => e.ExpectedArrival)
                .HasColumnName("expected_arrival");

            entity.Property(e => e.ActualArrival)
                .HasColumnName("actual_arrival");

            entity.Property(e => e.OrderStatus)
                .HasMaxLength(45)
                .HasColumnName("order_status");
        });
    }
}
