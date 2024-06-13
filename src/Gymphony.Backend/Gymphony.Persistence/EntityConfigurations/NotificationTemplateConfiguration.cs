using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.HasKey(notification => notification.Id);
        builder.HasIndex(notification => notification.Type).IsUnique();
        builder.Property(notification => notification.Title).HasMaxLength(256).IsRequired();
        builder.Property(notification => notification.Content).IsRequired();
    }
}