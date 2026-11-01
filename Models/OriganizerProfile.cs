namespace EventHub.Models;

public class OrganizerProfile
{
    public int Id { get; set; }

    public string? Biography { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? LogoUrl { get; set; }

    // Foreign key
    public int OrganizerId { get; set; }

    // Navigation property
    public Organizer Organizer { get; set; } = null!;
}