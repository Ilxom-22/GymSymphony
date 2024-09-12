using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.Property(staff => staff.Bio).IsRequired().HasMaxLength(2048);

        builder.Property(staff => staff.TemporaryPasswordChanged).HasColumnName("TemporaryPasswordChanged");
    }
}