using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.Property(admin => admin.TemporaryPasswordChanged).HasColumnName("TemporaryPasswordChanged");

        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(admin => admin.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}