using EventHub.Configurations;
using EventHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Data;

public class EventHubContext : DbContext
{
    private readonly string _connectionString;

    public EventHubContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<Organizer> Organizers => Set<Organizer>();

    public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();

    public DbSet<Event> Events => Set<Event>();

    public DbSet<Attendee> Attendees => Set<Attendee>();

    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API through separate configuration classes.
        modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());

        // Fluent API directly inside OnModelCreating.
        modelBuilder.Entity<Badge>(builder =>
        {
            builder.HasKey(badge => badge.Id);

            builder.Property(badge => badge.BadgeNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(badge => badge.BadgeNumber)
                .IsUnique();

            builder.Property(badge => badge.IssueDate)
                .IsRequired();

            builder.Property(badge => badge.Tier)
                .IsRequired();

            builder.HasOne(badge => badge.Attendee)
                .WithOne(attendee => attendee.Badge)
                .HasForeignKey<Badge>(badge => badge.AttendeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}