using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.HasKey(token => token.UserId);
        builder.HasIndex(token => token.Token).IsUnique();
        builder.Property(token => token.ExpiryTime).IsRequired();

        builder
            .HasOne<User>(token => token.User)
            .WithOne()
            .HasForeignKey<AccessToken>(token => token.UserId);
    }
}