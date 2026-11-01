using EventHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Configurations;

public class OrganizerProfileConfiguration
    : IEntityTypeConfiguration<OrganizerProfile>
{
    public void Configure(EntityTypeBuilder<OrganizerProfile> builder)
    {
        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Biography)
            .IsRequired(false);

        builder.Property(profile => profile.WebsiteUrl)
            .IsRequired(false);

        builder.Property(profile => profile.LogoUrl)
            .IsRequired(false);

        builder.HasOne(profile => profile.Organizer)
            .WithOne(organizer => organizer.Profile)
            .HasForeignKey<OrganizerProfile>(profile => profile.OrganizerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}