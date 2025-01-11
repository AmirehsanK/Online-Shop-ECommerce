using Domain.Entities.ContactUs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Subject).IsRequired();
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Message).IsRequired();
        builder.Property(m => m.IsAnswered).HasDefaultValue(false);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(m => m.AdminResponse).HasMaxLength(500);
        builder.Property(m => m.RespondedAt).IsRequired(false);
    }
}