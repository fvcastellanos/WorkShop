using Microsoft.EntityFrameworkCore;

namespace WorkShop.Model
{
    public class WorkShopContext: DbContext
    {
        private const string WorkShopSchema = "workshop";
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<DiscountType> DiscountTypes { get; set; }
        public virtual DbSet<OperationType> OperationTypes { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderInvoice> ProviderInvoices { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        public WorkShopContext(DbContextOptions options) : base(options)
        {
            // db context initialize
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(WorkShopSchema);

            // Product
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .HasName("uq_product_code")
                .IsUnique(true);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Created)
                .HasName("idx_product_created");

            modelBuilder.Entity<Product>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Updated)
                .HasName("idx_product_updated");

            modelBuilder.Entity<Product>()
                .Property(p => p.MinimalAmount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Product>()
                .Property(p => p.Active)
                .HasDefaultValue(1);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_product_tenant");

            // Discount Type
            modelBuilder.Entity<DiscountType>()
                .HasIndex(p => p.Created)
                .HasName("idx_discount_type_created");

            modelBuilder.Entity<DiscountType>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<DiscountType>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<DiscountType>()
                .HasIndex(p => p.Updated)
                .HasName("idx_discount_type_updated");

            modelBuilder.Entity<DiscountType>()
                .Property(p => p.Active)
                .HasDefaultValue(1);

            modelBuilder.Entity<DiscountType>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_discount_type_tenant");


            // Operation Type
            modelBuilder.Entity<OperationType>()
                .HasIndex(p => p.Created)
                .HasName("idx_operation_type_created");

            modelBuilder.Entity<OperationType>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<OperationType>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<OperationType>()
                .HasIndex(p => p.Updated)
                .HasName("idx_operation_type_updated");

            modelBuilder.Entity<OperationType>()
                .Property(p => p.Active)
                .HasDefaultValue(1);

            modelBuilder.Entity<OperationType>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_operation_type_tenant");

            modelBuilder.Entity<OperationType>()
                .Property(p => p.Inbound)
                .HasDefaultValueSql("0");
            
            modelBuilder.Entity<OperationType>()
                .HasIndex(p => p.Inbound)
                .HasName("idx_operation_type_inbound");

            // Provider
            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Code)
                .HasName("uq_provider_code")
                .IsUnique(true);

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.TaxId)
                .HasName("idx_provider_tax_id");

            modelBuilder.Entity<Provider>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Provider>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Created)
                .HasName("idx_provider_created");

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Updated)
                .HasName("idx_provider_updated");

            modelBuilder.Entity<Provider>()
                .Property(p => p.Active)
                .HasDefaultValue(1);

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_provider_tenant");

            // Provider Invoice
            modelBuilder.Entity<ProviderInvoice>()
                .HasIndex(p => new { p.Serial, p.Number })
                .HasName("idx_provider_invoice_number");

            modelBuilder.Entity<ProviderInvoice>()
                .HasIndex(p => p.Active)
                .HasName("idx_provider_invoice_active");

            modelBuilder.Entity<ProviderInvoice>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ProviderInvoice>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ProviderInvoice>()
                .HasIndex(p => p.Created)
                .HasName("idx_provider_invoice_created");

            modelBuilder.Entity<ProviderInvoice>()
                .HasIndex(p => p.Updated)
                .HasName("idx_provider_invoice_updated");

            modelBuilder.Entity<ProviderInvoice>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_provider_invoice_tenant");

            // Inventory
            modelBuilder.Entity<Inventory>()
                .Property(p => p.Amount)
                .HasDefaultValue(1);

            modelBuilder.Entity<Inventory>()
                .Property(p => p.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Inventory>()
                .Property(p => p.Updated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Inventory>()
                .HasIndex(p => p.Created)
                .HasName("idx_inventory_created");

            modelBuilder.Entity<Inventory>()
                .HasIndex(p => p.Updated)
                .HasName("idx_inventory_updated");

            modelBuilder.Entity<Inventory>()
                .HasIndex(p => p.Tenant)
                .HasName("idx_inventory_tenant");

            // UserToken
            modelBuilder.Entity<UserToken>()
                .HasIndex(p => p.User)
                    .IsUnique(true)
                    .HasName("uq_user_token_user");
        }
    }
}