using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.HasIndex(product => product.Name).IsUnique();
        builder.Property(plan => plan.Name).HasMaxLength(256);
        builder.Property(plan => plan.Description).IsRequired().HasMaxLength(2048);
        builder.Property(plan => plan.Price).IsRequired().HasColumnType("numeric(18,2)");

        builder.OwnsOne(plan => plan.StripeDetails, conf =>
        {
            conf.WithOwner().HasForeignKey("Id");
            conf.Property(sd => sd.ProductId).HasColumnName("StripeProductId").IsRequired();
            conf.Property(sd => sd.PriceId).HasColumnName("StripePriceId").IsRequired();
        });
        
        builder.HasOne<Admin>(plan => plan.CreatedBy)
            .WithMany()
            .HasForeignKey(plan => plan.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Admin>(plan => plan.ModifiedBy)
            .WithMany()
            .HasForeignKey(plan => plan.ModifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasDiscriminator(product => product.Type)
            .HasValue<MembershipPlan>(ProductType.MembershipPlan)
            .HasValue<Course>(ProductType.Course);
    }
}