using System.Runtime;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.EmailAddress).IsUnique();
        builder.Property(user => user.FirstName).HasMaxLength(64).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(64);
        builder.Property(user => user.AuthDataHash).IsRequired();
        
        builder.HasDiscriminator(user => user.Role)
            .HasValue<Admin>(Role.Admin)
            .HasValue<Staff>(Role.Staff)
            .HasValue<Member>(Role.Member);
    }
}