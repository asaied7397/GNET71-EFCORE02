namespace EventHub.Models;

public class Badge
{
    public int Id { get; set; }

    public string BadgeNumber { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public BadgeTier Tier { get; set; }

    // Foreign key
    public int AttendeeId { get; set; }

    // Navigation property
    public Attendee Attendee { get; set; } = null!;
}