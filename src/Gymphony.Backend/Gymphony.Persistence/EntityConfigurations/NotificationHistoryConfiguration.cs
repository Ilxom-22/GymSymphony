using Gymphony.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymphony.Persistence.EntityConfigurations;

public class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
{
    public void Configure(EntityTypeBuilder<NotificationHistory> builder)
    {
        builder.HasKey(notification => notification.Id);
        builder.Property(notification => notification.Title).HasMaxLength(256).IsRequired();
        builder.Property(notification => notification.Content).IsRequired();
        builder.Property(notification => notification.TemplateId).IsRequired();
        builder.Property(notification => notification.RecipientId).IsRequired();

        builder
            .HasOne<NotificationTemplate>(history => history.Template)
            .WithMany()
            .HasForeignKey(history => history.TemplateId);
    }
}