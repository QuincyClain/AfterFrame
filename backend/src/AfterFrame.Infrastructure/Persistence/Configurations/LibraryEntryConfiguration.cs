using AfterFrame.Domain.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfterFrame.Infrastructure.Persistence.Configurations;

internal sealed class LibraryEntryConfiguration
    : IEntityTypeConfiguration<LibraryEntry>
{
    public void Configure(EntityTypeBuilder<LibraryEntry> builder)
    {
        builder.ToTable("library_entries", table =>
            {
                table.HasCheckConstraint("ck_library_entries_rating",
                    """
                    "rating" IS NULL
                    OR
                    (
                        "rating" >= 0.5
                        AND "rating" <= 10
                        AND MOD("rating" * 2, 1) = 0
                    )
                    """);

                table.HasCheckConstraint("ck_library_entries_progress",
                    """
                    (
                        "last_watched_season" IS NULL
                        AND "last_watched_episode" IS NULL
                    )
                    OR
                    (
                        "last_watched_season" IS NOT NULL
                        AND "last_watched_season" > 0
                        AND "last_watched_episode" IS NOT NULL
                        AND "last_watched_episode" > 0
                    )
                    """);
            });

        builder.HasKey(entry => new { entry.UserId, entry.TitleId });

        builder.Property(entry => entry.UserId)
            .HasColumnName("user_id");

        builder.Property(entry => entry.TitleId)
            .HasColumnName("title_id");

        builder.Property(entry => entry.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(entry => entry.Rating)
            .HasColumnName("rating")
            .HasPrecision(3, 1);

        builder.Property(entry => entry.Review)
            .HasColumnName("review")
            .HasMaxLength(2000);

        builder.Property(entry => entry.LastWatchedSeason)
            .HasColumnName("last_watched_season");

        builder.Property(entry => entry.LastWatchedEpisode)
            .HasColumnName("last_watched_episode");

        builder.Property(entry => entry.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(entry => entry.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasOne(entry => entry.User)
            .WithMany()
            .HasForeignKey(entry => entry.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(entry => entry.Title)
            .WithMany()
            .HasForeignKey(entry => entry.TitleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(entry => new { entry.UserId, entry.Status })
            .HasDatabaseName("ix_library_entries_user_id_status");

        builder.HasIndex(entry => entry.TitleId)
            .HasDatabaseName("ix_library_entries_title_id");
    }
}