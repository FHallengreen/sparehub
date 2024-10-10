using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class SpareHubDbContext(DbContextOptions<SpareHubDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Vessel> Vessels => Set<Vessel>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();
    public DbSet<DispatchStatus> DispatchStatuses => Set<DispatchStatus>();
    public DbSet<Dispatch> Dispatches => Set<Dispatch>();
    public DbSet<TransportMode> TransportModes => Set<TransportMode>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<CostType> CostTypes => Set<CostType>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Financial> Financials => Set<Financial>();
    public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many: Order and Dispatch
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Dispatches)
            .WithMany(d => d.Orders)
            .UsingEntity<Dictionary<string, object>>(
                "DispatchHasOrder",
                j => j.HasOne<Dispatch>().WithMany().HasForeignKey("DispatchId"),
                j => j.HasOne<Order>().WithMany().HasForeignKey("OrderId"));

        // Order Configuration
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
                .HasColumnName("order_status")
                .IsRequired();

            // Explicit Foreign Key Mapping
            entity.HasOne(e => e.Supplier)
                .WithMany()
                .HasForeignKey(e => e.SupplierId)
                .HasConstraintName("fk_Order_Supplier")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Vessel)
                .WithMany()
                .HasForeignKey(e => e.VesselId)
                .HasConstraintName("fk_Order_Vessel1")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Warehouse)
                .WithMany()
                .HasForeignKey(e => e.WarehouseId)
                .HasConstraintName("fk_Order_Warehouse1")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.OrderStatus)
                .HasDatabaseName("fk_order_order_status1_idx");

            entity.HasOne<OrderStatus>()
                .WithMany()
                .HasForeignKey(e => e.OrderStatus) // This maps Order.OrderStatus to OrderStatus.Status
                .HasConstraintName("fk_order_order_status1")
                .OnDelete(DeleteBehavior.NoAction);
        });


        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("order_status");

            entity.HasKey(e => e.Status);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(45)
                .IsRequired();
        });

        // Dispatch Configuration
        // Dispatch Configuration
        modelBuilder.Entity<Dispatch>(entity =>
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

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("fk_dispatch_user1")
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Supplier Configuration
        modelBuilder.Entity<Supplier>(entity =>
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

            entity.HasOne<Address>()
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .HasConstraintName("fk_Supplier_Address1")
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Vessel Configuration
        modelBuilder.Entity<Vessel>(entity =>
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
                .HasColumnName("owner_id");

            entity.Property(e => e.ImoNumber)
                .HasColumnName("imo_number")
                .HasMaxLength(7);

            entity.Property(e => e.Flag)
                .HasColumnName("flag")
                .HasMaxLength(3);

            entity.HasOne<Owner>()
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .HasConstraintName("fk_Vessel_Owner1")
                .OnDelete(DeleteBehavior.NoAction);
        });
        // Agent Configuration
        modelBuilder.Entity<Agent>(entity =>
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
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("warehouse");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();

            entity.Property(e => e.AgentId)
                .HasColumnName("agent_id");

            entity.HasOne<Agent>()
                .WithMany()
                .HasForeignKey(e => e.AgentId)
                .HasConstraintName("fk_Warehouse_Agent1")
                .OnDelete(DeleteBehavior.NoAction);
        });
        // DispatchStatus Configuration
        modelBuilder.Entity<DispatchStatus>(entity =>
        {
            entity.ToTable("dispatch_status");

            entity.HasKey(e => e.Status);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(45)
                .IsRequired();
        });

        // Role Configuration
        modelBuilder.Entity<Role>(entity =>
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
        });
        // User Configuration
        modelBuilder.Entity<User>(entity =>
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

            entity.HasOne<Role>()
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("fk_Operator_Role1")
                .OnDelete(DeleteBehavior.NoAction);
        });
        // ContactInfo Configuration
        modelBuilder.Entity<ContactInfo>(entity =>
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
        modelBuilder.Entity<TransportMode>(entity =>
        {
            entity.ToTable("transport_mode");

            entity.HasKey(e => e.Type);
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(10)
                .IsRequired();
        });
        // Invoice Configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("invoice");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DispatchId)
                .HasColumnName("dispatch_id");

            entity.HasOne<Dispatch>()
                .WithMany()
                .HasForeignKey(e => e.DispatchId)
                .HasConstraintName("fk_Invoice_Dispatch1")
                .OnDelete(DeleteBehavior.NoAction);
        });
        // CostType Configuration
        modelBuilder.Entity<CostType>(entity =>
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
        modelBuilder.Entity<Currency>(entity =>
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
        modelBuilder.Entity<Financial>(entity =>
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

            entity.HasOne(e => e.Invoice)
                .WithMany() 
                .HasForeignKey(e => e.InvoiceId)
                .HasConstraintName("fk_financial_invoice1")
                .OnDelete(DeleteBehavior.NoAction);

          
            entity.HasOne(e => e.CostType)
                .WithMany()  
                .HasForeignKey(e => e.CostTypeId) 
                .HasConstraintName("fk_financial_cost_type1")
                .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(e => e.Currency)
                .WithMany()  
                .HasForeignKey(e => e.CurrencyId) 
                .HasConstraintName("fk_financial_currency1")
                .OnDelete(DeleteBehavior.NoAction);
        });

    }
}
