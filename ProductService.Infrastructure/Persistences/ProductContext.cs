using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistences
{
    public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
    {

        // Define DbSet properties for your entities here
        // public DbSet<Product> Products { get; set; }
        // public DbSet<Category> Categories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<SKU> SKUs { get; set; }
        public DbSet<SKUAttribute> SKUAtrributes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.CategoryId, p.Price })
                .HasDatabaseName("IX_CategoryId_Price");

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .OwnsMany(p => p.ProductAttributes, pa =>
                {
                    pa.WithOwner().HasForeignKey("ProductId");
                    pa.HasKey("ProductId", "AttributeName");
                    pa.Property(pa => pa.AttributeValue).IsRequired();
                });
            modelBuilder.Entity<Product>()
                .OwnsOne(typeof(ProductRatingSummary), "_ratingSummary", pr =>
                {
                    pr.WithOwner().HasForeignKey("ProductId");
                    pr.HasKey("ProductId");
                    pr.Property("AverageRating").IsRequired();
                    pr.Property("TotalReviews").IsRequired();
                });

            modelBuilder.Entity<Discount>()
                .HasKey(d => d.DiscountId);

            modelBuilder.Entity<ProductDiscount>()
                .HasKey(pd => pd.ProductDiscountId);

            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Product)
                .WithMany(p => p.ProductDiscounts)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Discount)
                .WithMany(d => d.ProductDiscounts)
                .HasForeignKey(pd => pd.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.ProductId, r.CreatedAt })
                .HasDatabaseName("IX_ProductId_CreateAt");

            modelBuilder.Entity<SKU>()
                .HasKey(s => s.SKUId);
            modelBuilder.Entity<SKU>()
                .HasOne(s => s.Product)
                .WithMany(p => p.SKUs)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SKU>()
                .HasIndex(s => s.ProductId)
                .HasDatabaseName("IX_SKU_ProductId");

            modelBuilder.Entity<SKUAttribute>()
                .HasKey(s => s.SKUAttributeId);
            modelBuilder.Entity<SKUAttribute>()
                .HasOne(s => s.SKU)
                .WithMany(s => s.SKUAttributes)
                .HasForeignKey(s => s.SKUId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SKUAttribute>()
                .HasIndex(s => s.SKUId)
                .HasDatabaseName("IX_SKUAttribute_SKUId");
        }
    }
}
