using EventHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(eventEntity => eventEntity.Id);

        builder.Property(eventEntity => eventEntity.Title)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.Description)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.StartDate)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.EndDate)
            .IsRequired(false);

        builder.Property(eventEntity => eventEntity.MaxAttendees)
            .IsRequired();

        builder.HasOne(eventEntity => eventEntity.Organizer)
            .WithMany(organizer => organizer.Events)
            .HasForeignKey(eventEntity => eventEntity.OrganizerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}