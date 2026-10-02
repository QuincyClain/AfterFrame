using AfterFrame.Domain.Titles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfterFrame.Infrastructure.Persistence.Configurations;

internal sealed class TitleConfiguration
    : IEntityTypeConfiguration<Title>
{
    public void Configure(EntityTypeBuilder<Title> builder)
    {
        builder.ToTable("titles", table =>
        {
            table.HasCheckConstraint("ck_titles_release_year", "\"release_year\" > 0");

            table.HasCheckConstraint(
                "ck_titles_origin",
                """
                (
                    "origin" = 'External'
                    AND "publication_status" = 'Published'
                    AND "external_source" IS NOT NULL
                    AND "external_id" IS NOT NULL
                    AND "created_by_user_id" IS NULL
                )
                OR
                (
                    "origin" = 'UserCreated'
                    AND "external_source" IS NULL
                    AND "external_id" IS NULL
                    AND "created_by_user_id" IS NOT NULL
                )
                """);

            table.HasCheckConstraint(
                "ck_titles_external_rating",
                """
                (
                    "origin" = 'External'
                    AND
                    (
                        (
                            "external_rating" IS NULL
                            AND "external_vote_count" = 0
                        )
                        OR
                        (
                            "external_rating" IS NOT NULL
                            AND "external_rating" >= 0
                            AND "external_rating" <= 10
                            AND "external_vote_count" > 0
                        )
                    )
                )
                OR
                (
                    "origin" = 'UserCreated'
                    AND "external_rating" IS NULL
                    AND "external_vote_count" = 0
                )
                """);
        });

        builder.HasKey(title => title.Id);

        builder.Property(title => title.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(title => title.Name)
            .HasColumnName("name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(title => title.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(title => title.Tags)
            .HasColumnName("tags")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(title => title.ReleaseYear)
            .HasColumnName("release_year")
            .IsRequired();

        builder.Property(title => title.Description)
            .HasColumnName("description")
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(title => title.PosterPath)
            .HasColumnName("poster_path")
            .HasMaxLength(500);

        builder.Property(title => title.BackdropPath)
            .HasColumnName("backdrop_path")
            .HasMaxLength(500);

        builder.Property(title => title.ExternalRating)
            .HasColumnName("external_rating")
            .HasPrecision(5, 3);

        builder.Property(title => title.ExternalVoteCount)
            .HasColumnName("external_vote_count")
            .IsRequired();

        builder.Property(title => title.Origin)
            .HasColumnName("origin")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(title => title.PublicationStatus)
            .HasColumnName("publication_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(title => title.ExternalSource)
            .HasColumnName("external_source")
            .HasMaxLength(50);

        builder.Property(title => title.ExternalId)
            .HasColumnName("external_id")
            .HasMaxLength(100);

        builder.Property(title => title.CreatedByUserId)
            .HasColumnName("created_by_user_id");

        builder.Property(title => title.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(title => title.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasOne(title => title.CreatedByUser)
            .WithMany()
            .HasForeignKey(title => title.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(title => title.Genres)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(title => title.Genres)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>("TitleGenre", right => right
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("genre_id")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Title>()
                    .WithMany()
                    .HasForeignKey("title_id")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("title_genres");

                    join.HasKey("title_id", "genre_id");

                    join.HasIndex("genre_id")
                        .HasDatabaseName("ix_title_genres_genre_id");
                });

        builder.HasIndex(title => new { title.ExternalSource, title.ExternalId })
            .IsUnique()
            .HasDatabaseName("ux_titles_external_identity")
            .HasFilter("\"external_source\" IS NOT NULL AND \"external_id\" IS NOT NULL");

        builder.HasIndex(title => title.CreatedByUserId)
            .HasDatabaseName("ix_titles_created_by_user_id");
    }
}