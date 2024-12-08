namespace Infra.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.ContactUs;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property((m => m.Subject)).IsRequired();
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Message).IsRequired();
        builder.Property(m => m.IsAnswered).HasDefaultValue(false);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("GETDATE()");
    }
}