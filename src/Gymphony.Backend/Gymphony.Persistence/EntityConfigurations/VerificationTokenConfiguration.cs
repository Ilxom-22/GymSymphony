using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
{
    public void Configure(EntityTypeBuilder<VerificationToken> builder)
    {
        builder.HasKey(token => token.Id);

        builder.HasIndex(token => token.Token).IsUnique();

        builder.HasOne<User>(token => token.User)
            .WithOne(user => user.VerificationToken)
            .HasForeignKey<VerificationToken>(token => token.UserId);
    }
}