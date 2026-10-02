using AfterFrame.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AfterFrame.Infrastructure.Identity;

namespace AfterFrame.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(user => user.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();
        
        builder.HasData(new
        {
            Id = DevelopmentUserDefaults.Id,
            DisplayName = DevelopmentUserDefaults.DisplayName,
            CreatedAtUtc = DevelopmentUserDefaults.CreatedAtUtc
        });
    }
}