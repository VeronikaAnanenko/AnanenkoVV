using BSUIR.DAL.Interfaces.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSUIR.DAL
{
    public partial class InternetShopContext : IdentityDbContext<User>
    {
        public InternetShopContext()
        {
        }

        public InternetShopContext(DbContextOptions<InternetShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DeliveryAddress> DeliveryAddress { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderHasItem> OrderHasItem { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InternetShop");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.CustomerId).HasColumnName("customer_id").HasMaxLength(500)
                    .IsUnicode(true);

                entity.Property(e => e.Flat).HasColumnName("flat");

                entity.Property(e => e.House).HasColumnName("house");

                entity.Property(e => e.PostIndex).HasColumnName("post_index");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasColumnName("region")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_customer");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.MobileNumber)
                    .HasColumnName("mobile_number")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Secondname)
                    .HasColumnName("secondname")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasMaxLength(45)
                    .IsUnicode(true);
                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_user");
            });

            modelBuilder.Entity<DeliveryAddress>(entity =>
            {
                entity.ToTable("delivery_address");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Coordinates)
                    .IsRequired()
                    .HasColumnName("coordinates")
                    .HasMaxLength(200)
                    .IsUnicode(true);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Flat).HasColumnName("flat");

                entity.Property(e => e.House).HasColumnName("house");

                entity.Property(e => e.PostIndex).HasColumnName("post_index");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasColumnName("region")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.WorkingHours)
                    .IsRequired()
                    .HasColumnName("working_hours")
                    .HasMaxLength(100)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Discounts>(entity =>
            {
                entity.ToTable("discounts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.From)
                    .HasColumnName("from")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.To)
                    .HasColumnName("to")
                    .HasColumnType("decimal(10, 0)");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(5000)
                    .IsUnicode(true);

                entity.Property(e => e.Guarantee).HasColumnName("guarantee");

                entity.Property(e => e.Height)
                    .HasColumnName("height")
                    .HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Length)
                    .HasColumnName("length")
                    .HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Material)
                    .IsRequired()
                    .HasColumnName("material")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Producer)
                    .HasColumnName("producer")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.Property(e => e.Validity).HasColumnName("validity");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight")
                    .HasColumnType("decimal(14, 0)");

                entity.Property(e => e.Width)
                    .HasColumnName("width")
                    .HasColumnType("decimal(12, 0)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_item_category");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(500)
                    .IsUnicode(true);

                entity.Property(e => e.CustomerId).HasColumnName("customer_id").HasMaxLength(500)
                    .IsUnicode(true);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.DeliveryAddressId).HasColumnName("delivery_address_id");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_customer");

                entity.HasOne(d => d.DeliveryAddress)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.DeliveryAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_delivery_address_id");
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_address");
            });

            modelBuilder.Entity<OrderHasItem>(entity =>
            {
                entity.ToTable("order_has_item");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_has_item_item");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_has_item_order");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnName("link")
                    .HasMaxLength(200)
                    .IsUnicode(true);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(true);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_photo_item");
            });
            base.OnModelCreating(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
