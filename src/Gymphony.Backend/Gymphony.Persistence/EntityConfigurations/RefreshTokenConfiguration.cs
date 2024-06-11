using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(token => token.UserId);
        builder.HasIndex(token => token.Token).IsUnique();
        builder.Property(token => token.ExpiryTime).IsRequired();

        builder
            .HasOne<User>(token => token.User)
            .WithOne(user => user.RefreshToken)
            .HasForeignKey<RefreshToken>(token => token.UserId);
    }
}