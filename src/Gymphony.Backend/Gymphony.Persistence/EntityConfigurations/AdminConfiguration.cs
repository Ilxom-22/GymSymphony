using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasOne<Admin>()
            .WithMany()
            .HasForeignKey(admin => admin.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}