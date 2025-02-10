using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("sales", schema: "public");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.TotalSaleAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.Branch)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.IsCancelled)
                .IsRequired();

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId);

            builder.Property(e => e.CreatedAt).IsRequired(true);
            builder.Property(e => e.UpdatedAt).IsRequired(false);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(e => e.UpdatedBy).HasMaxLength(100).IsRequired(false);
        }
    }

    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("sale_items", schema: "public");

            builder.HasKey(si => new { si.SaleId, si.ProductId });

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId);
        }
    }
}