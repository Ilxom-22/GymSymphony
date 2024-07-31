using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(payment => payment.Id);
        builder.Property(payment => payment.Amount).IsRequired();
        builder.Property(payment => payment.Date).IsRequired();

        builder
            .HasOne<Member>(payment => payment.Member)
            .WithMany()
            .HasForeignKey(payment => payment.MemberId);
    }
}