using AfterFrame.Application.Common.Persistence;
using AfterFrame.Domain.Library;
using AfterFrame.Domain.Titles;
using AfterFrame.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence;

public sealed class AfterFrameDbContext(
    DbContextOptions<AfterFrameDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Title> Titles => Set<Title>();

    public DbSet<LibraryEntry> LibraryEntries => Set<LibraryEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AfterFrameDbContext).Assembly);
    }
}