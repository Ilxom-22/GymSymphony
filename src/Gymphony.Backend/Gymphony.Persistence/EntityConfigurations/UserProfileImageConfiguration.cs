using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class UserProfileImageConfiguration : IEntityTypeConfiguration<UserProfileImage>
{
    public void Configure(EntityTypeBuilder<UserProfileImage> builder)
    {
        builder.HasKey(upi => upi.Id);

        builder.HasOne(upi => upi.StorageFile)
            .WithOne()
            .HasForeignKey<UserProfileImage>(upi => upi.StorageFileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(upi => upi.User)
            .WithOne(user => user.ProfileImage)
            .HasForeignKey<UserProfileImage>(upi => upi.UserId);
    }
}
