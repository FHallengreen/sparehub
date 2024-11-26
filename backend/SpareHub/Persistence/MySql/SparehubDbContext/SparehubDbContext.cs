using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.MySql.SparehubDbContext;

public class SpareHubDbContext(DbContextOptions<SpareHubDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<SupplierEntity> Suppliers => Set<SupplierEntity>();
    public DbSet<OwnerEntity> Owners => Set<OwnerEntity>();
    public DbSet<VesselEntity> Vessels => Set<VesselEntity>();
    public DbSet<AgentEntity> Agents => Set<AgentEntity>();
    public DbSet<WarehouseEntity> Warehouses => Set<WarehouseEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ContactInfoEntity> ContactInfos => Set<ContactInfoEntity>();
    public DbSet<DispatchStatusEntity> DispatchStatuses => Set<DispatchStatusEntity>();
    public DbSet<DispatchEntity> Dispatches => Set<DispatchEntity>();
    public DbSet<TransportModeEntity> TransportModes => Set<TransportModeEntity>();
    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<CostTypeEntity> CostTypes => Set<CostTypeEntity>();
    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();
    public DbSet<FinancialEntity> Financials => Set<FinancialEntity>();
    public DbSet<OrderStatusEntity> OrderStatus => Set<OrderStatusEntity>();
    public DbSet<BoxEntity> Boxes => Set<BoxEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderEntity>(entity =>
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

            entity.HasOne(o => o.Supplier)
                .WithMany()
                .HasForeignKey(o => o.SupplierId)
                .HasConstraintName("fk_Order_Supplier")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(o => o.Vessel)
                .WithMany()
                .HasForeignKey(o => o.VesselId)
                .HasConstraintName("fk_Order_Vessel")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(o => o.Warehouse)
                .WithMany()
                .HasForeignKey(o => o.WarehouseId)
                .HasConstraintName("fk_Order_Warehouse")
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.VesselId).HasColumnName("vessel_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.ExpectedReadiness).HasColumnName("expected_readiness");
            entity.Property(e => e.ActualReadiness).HasColumnName("actual_readiness");
            entity.Property(e => e.ExpectedArrival).HasColumnName("expected_arrival");
            entity.Property(e => e.ActualArrival).HasColumnName("actual_arrival");
            entity.Property(e => e.OrderStatus)
                .HasColumnName("order_status")
                .IsRequired();

            entity.HasMany(o => o.Boxes)
                .WithOne(b => b.Order)
                .HasForeignKey(b => b.OrderId)
                .HasConstraintName("fk_box_order1")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<BoxEntity>(entity =>
        {
            entity.ToTable("box");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("CHAR(36)")
                .IsRequired();

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            entity.Property(e => e.Length)
                .HasColumnName("length")
                .IsRequired();

            entity.Property(e => e.Width)
                .HasColumnName("width")
                .IsRequired();

            entity.Property(e => e.Height)
                .HasColumnName("height")
                .IsRequired();

            entity.Property(e => e.Weight)
                .HasColumnName("weight")
                .IsRequired();
        });

// Port Configuration
        modelBuilder.Entity<PortEntity>(entity =>
        {
            entity.ToTable("port");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();
        });

// VesselAtPort Configuration
        modelBuilder.Entity<VesselAtPortEntity>(entity =>
        {
            entity.ToTable("vessel_at_port");

            entity.HasKey(e => new { e.VesselId, e.PortId });

            entity.Property(e => e.VesselId)
                .HasColumnName("vessel_id")
                .IsRequired();

            entity.Property(e => e.PortId)
                .HasColumnName("port_id")
                .IsRequired();

            entity.Property(e => e.ArrivalDate)
                .HasColumnName("arrival_date");

            entity.Property(e => e.DepartureDate)
                .HasColumnName("departure_date");

            entity.HasOne(e => e.VesselEntity)
                .WithMany(v => v.VesselAtPorts)
                .HasForeignKey(e => e.VesselId)
                .HasConstraintName("fk_vessel_has_port_vessel1")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.PortEntity)
                .WithMany(p => p.VesselAtPorts)
                .HasForeignKey(e => e.PortId)
                .HasConstraintName("fk_vessel_has_port_port1")
                .OnDelete(DeleteBehavior.NoAction);
        });

// Vessel Configuration
        modelBuilder.Entity<VesselEntity>(entity =>
        {
            entity.ToTable("vessel");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();

            entity.Property(e => e.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            entity.Property(e => e.ImoNumber)
                .HasColumnName("imo_number")
                .HasMaxLength(7);

            entity.Property(e => e.Flag)
                .HasColumnName("flag")
                .HasMaxLength(3);

            entity.HasOne(e => e.Owner)
                .WithMany(o => o.Vessels)
                .HasForeignKey(e => e.OwnerId)
                .HasConstraintName("fk_Vessel_Owner1")
                .OnDelete(DeleteBehavior.NoAction);
        });


        modelBuilder.Entity<OrderStatusEntity>(entity =>
        {
            entity.ToTable("order_status");

            entity.HasKey(e => e.Status);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(45)
                .IsRequired();
        });

// Dispatch Configuration
        modelBuilder.Entity<DispatchEntity>(entity =>
        {
            entity.ToTable("dispatch");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.OriginType)
                .HasColumnName("origin_type")
                .IsRequired();

            entity.Property(e => e.OriginId)
                .HasColumnName("origin_id")
                .IsRequired();

            entity.Property(e => e.DestinationType)
                .HasColumnName("destination_type")
                .IsRequired();

            entity.Property(e => e.DestinationId)
                .HasColumnName("destination_id");

            entity.Property(e => e.DispatchStatus)
                .HasColumnName("dispatch_status")
                .IsRequired();

            entity.Property(e => e.TransportModeType)
                .HasColumnName("transport_mode_type")
                .IsRequired();

            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(45)
                .HasColumnName("tracking_number");

            entity.Property(e => e.DispatchDate)
                .HasColumnName("dispatch_date");

            entity.Property(e => e.DeliveryDate)
                .HasColumnName("delivery_date");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.HasOne(e => e.userEntity)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("fk_dispatch_user1")
                .OnDelete(DeleteBehavior.NoAction);

            // Configure many-to-many relationship with Order
            entity.HasMany(d => d.Orders)
                .WithMany(o => o.Dispatches)
                .UsingEntity<Dictionary<string, object>>(
                    "dispatch_has_order",
                    j => j.HasOne<OrderEntity>().WithMany().HasForeignKey("order_id"),
                    j => j.HasOne<DispatchEntity>().WithMany().HasForeignKey("dispatch_id"),
                    j =>
                    {
                        j.ToTable("dispatch_has_order");
                        j.HasKey("dispatch_id", "order_id");
                    });
        });


// Owner Configuration
        modelBuilder.Entity<OwnerEntity>(entity =>
        {
            entity.ToTable("owner");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(45);

            entity.HasMany(o => o.Vessels)
                .WithOne(v => v.Owner)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_vessel_owner");
        });


        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.ToTable("supplier");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();

            entity.Property(e => e.AddressId)
                .HasColumnName("address_id");

            entity.HasOne(e => e.AddressEntity)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .HasConstraintName("fk_Supplier_Address1")
                .OnDelete(DeleteBehavior.NoAction);
        });

// Address Configuration
        modelBuilder.Entity<AddressEntity>(entity =>
        {
            entity.ToTable("address");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.AddressLine)
                .HasColumnName("address")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.PostalCode)
                .HasColumnName("postal_code")
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(e => e.Country)
                .HasColumnName("country")
                .HasMaxLength(45)
                .IsRequired();
        });

// Agent Configuration
        modelBuilder.Entity<AgentEntity>(entity =>
        {
            entity.ToTable("agent");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();
        });
// Warehouse Configuration
        modelBuilder.Entity<WarehouseEntity>(entity =>
        {
            entity.ToTable("warehouse");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(45);

            entity.Property(e => e.AgentId)
                .HasColumnName("agent_id");

            entity.HasOne(e => e.Agent)
                .WithMany()
                .HasForeignKey(e => e.AgentId)
                .HasConstraintName("fk_Warehouse_Agent")
                .OnDelete(DeleteBehavior.NoAction);
        });


// DispatchStatus Configuration
        modelBuilder.Entity<DispatchStatusEntity>(entity =>
        {
            entity.ToTable("dispatch_status");

            entity.HasKey(e => e.Status);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(45)
                .IsRequired();
        });

// Role Configuration
        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.ToTable("role");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(45)
                .IsRequired();
            
            entity.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("fk_User_Role")
                .OnDelete(DeleteBehavior.NoAction);
        });
// User Configuration
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("user");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();

            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");

            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("fk_User_Role")
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(e => e.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            entity.HasOne(u => u.Owner)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OwnerId)
                .HasConstraintName("fk_User_Owner")
                .OnDelete(DeleteBehavior.NoAction);
        });
// ContactInfo Configuration
        modelBuilder.Entity<ContactInfoEntity>(entity =>
        {
            entity.ToTable("contact_info");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45);

            entity.Property(e => e.Value)
                .HasColumnName("value")
                .HasMaxLength(45);

            entity.Property(e => e.ContactType)
                .HasColumnName("contact_type")
                .HasMaxLength(10);
        });
// TransportMode Configuration
        modelBuilder.Entity<TransportModeEntity>(entity =>
        {
            entity.ToTable("transport_mode");

            entity.HasKey(e => e.Type);
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(10)
                .IsRequired();
        });
        modelBuilder.Entity<InvoiceEntity>(entity =>
        {
            entity.ToTable("invoice");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DispatchId)
                .HasColumnName("dispatch_id");

            // Map the relationship
            entity.HasOne(e => e.DispatchEntity)
                .WithMany(d => d.Invoices)
                .HasForeignKey(e => e.DispatchId)
                .HasConstraintName("fk_Invoice_Dispatch1")
                .OnDelete(DeleteBehavior.NoAction);
        });

// CostType Configuration
        modelBuilder.Entity<CostTypeEntity>(entity =>
        {
            entity.ToTable("cost_type");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(45)
                .IsRequired();
        });
// Currency Configuration
        modelBuilder.Entity<CurrencyEntity>(entity =>
        {
            entity.ToTable("currency");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(3)
                .IsRequired();
        });
// Financial Configuration
        modelBuilder.Entity<FinancialEntity>(entity =>
        {
            entity.ToTable("financial");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.InvoiceId)
                .HasColumnName("invoice_id");

            entity.Property(e => e.CostTypeId)
                .HasColumnName("cost_type_id");

            entity.Property(e => e.CurrencyId)
                .HasColumnName("currency_id");

            entity.HasOne(e => e.InvoiceEntity)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .HasConstraintName("fk_financial_invoice1")
                .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(e => e.CostTypeEntity)
                .WithMany()
                .HasForeignKey(e => e.CostTypeId)
                .HasConstraintName("fk_financial_cost_type1")
                .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(e => e.CurrencyEntity)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .HasConstraintName("fk_financial_currency1")
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
