using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Models;

public class Attendee
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("ZipCode")]
    public string PostalCode { get; set; } = string.Empty;

    [NotMapped]
    public string FullAddress => $"{Street}, {City}, {Country}, {PostalCode}";

    // An attendee can have at most one badge.
    public Badge? Badge { get; set; }
}