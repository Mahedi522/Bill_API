using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bill_API.Models
{
    public partial class Bill_DBContext : DbContext
    {
        public Bill_DBContext()
        {
        }

        public Bill_DBContext(DbContextOptions<Bill_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<InventoryProduct> InventoryProducts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasIndex(e => e.BillNo, "UQ__Inventor__11F284184C0B619B")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventori__Custo__3A81B327");
            });

            modelBuilder.Entity<InventoryProduct>(entity =>
            {
                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InventoryProducts)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Inven__403A8C7D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Produ__412EB0B6");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.Code, "UQ__Products__A25C5AA7F14CE5C5")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
