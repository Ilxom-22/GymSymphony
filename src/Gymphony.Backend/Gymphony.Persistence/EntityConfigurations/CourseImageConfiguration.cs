using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class CourseImageConfiguration : IEntityTypeConfiguration<CourseImage>
{
    public void Configure(EntityTypeBuilder<CourseImage> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.HasOne(ci => ci.StorageFile)
            .WithOne()
            .HasForeignKey<CourseImage>(ci => ci.StorageFileId);

        builder.HasOne(ci => ci.Course)
            .WithMany(c => c.CourseImages)
            .HasForeignKey(ci => ci.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
