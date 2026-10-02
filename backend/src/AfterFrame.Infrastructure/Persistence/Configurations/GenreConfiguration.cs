using AfterFrame.Domain.Titles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfterFrame.Infrastructure.Persistence.Configurations;

internal sealed class GenreConfiguration
    : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("genres");

        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(genre => genre.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(genre => genre.Name)
            .IsUnique()
            .HasDatabaseName("ux_genres_name");
    }
}