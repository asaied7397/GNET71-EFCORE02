using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Models;

[Table("EventOrganizers")]
public class Organizer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? CompanyName { get; set; }

    // One organizer can have one profile.
    public OrganizerProfile? Profile { get; set; }

    // One organizer can publish many events.
    public ICollection<Event> Events { get; set; } = new List<Event>();
}