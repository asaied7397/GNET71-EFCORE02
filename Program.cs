using EventHub.Data;
using EventHub.Models;
using Microsoft.EntityFrameworkCore;

string? connectionString =
    Environment.GetEnvironmentVariable("EVENTHUB_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Set EVENTHUB_CONNECTION_STRING before running the application.");
    return;
}

try
{
    using var context = new EventHubContext(connectionString);

    // Create the database and schema when the database does not exist.
    bool databaseCreated = context.Database.EnsureCreated();

    Console.WriteLine(databaseCreated ? "Database schema created successfully." : "Database already exists.");

    // Add sample data only when the organizer is not already present.
    bool sampleExists = context.Organizers
        .Any(organizer => organizer.Name == "EventHub Demo Organizer");

    if (!sampleExists)
    {
        var organizer = new Organizer
        {
            Name = "EventHub Demo Organizer",
            CompanyName = "EventHub Events",

            Profile = new OrganizerProfile
            {
                Biography = "We organize technology events.",
                WebsiteUrl = "https://example.com",
                LogoUrl = "https://example.com/logo.png"
            },

            Events = new List<Event>
            {
                new Event
                {
                    Title = "Technology Conference",
                    Description = "A conference about modern technology.",
                    StartDate = new DateTime(2027, 5, 10, 9, 0, 0),
                    EndDate = new DateTime(2027, 5, 10, 17, 0, 0),
                    MaxAttendees = 200
                },

                new Event
                {
                    Title = "EF Core Workshop",
                    Description = "A practical workshop about Entity Framework Core.",
                    StartDate = new DateTime(2027, 6, 15, 10, 0, 0),
                    EndDate = null,
                    MaxAttendees = 50
                }
            }
        };

        var attendee = new Attendee
        {
            FullName = "Ahmed Saied",
            Email = "ahmed@example.com",
            Street = "123 Main Street",
            City = "Cairo",
            Country = "Egypt",
            PostalCode = "11511",

            Badge = new Badge
            {
                BadgeNumber = "EVH-0001",
                IssueDate = DateTime.UtcNow,
                Tier = BadgeTier.VIP
            }
        };

        context.Organizers.Add(organizer);
        context.Attendees.Add(attendee);
        context.SaveChanges();
        Console.WriteLine("Sample data added successfully.");
    }
    else
    {
        Console.WriteLine("Sample data already exists.");
    }

    Console.WriteLine();
    Console.WriteLine("===== Organizers and Events =====");
    var organizers = context.Organizers
        .AsNoTracking()
        .Include(organizer => organizer.Profile)
        .Include(organizer => organizer.Events)
        .OrderBy(organizer => organizer.Id)
        .ToList();

    foreach (var organizer in organizers)
    {
        Console.WriteLine($"Organizer: {organizer.Name}");
        Console.WriteLine($"Company: {organizer.CompanyName ?? "N/A"}");
        Console.WriteLine($"Biography: {organizer.Profile?.Biography ?? "N/A"}");
        Console.WriteLine($"Website: {organizer.Profile?.WebsiteUrl ?? "N/A"}");

        Console.WriteLine("Events:");

        foreach (var eventItem in organizer.Events.OrderBy(e => e.StartDate))
        {
            Console.WriteLine(
                $"- {eventItem.Title} | " +
                $"Starts: {eventItem.StartDate:yyyy-MM-dd HH:mm} | " +
                $"Capacity: {eventItem.MaxAttendees}"
            );
        }

        Console.WriteLine();
    }

    Console.WriteLine("===== Attendees and Badges =====");

    var attendees = context.Attendees
        .AsNoTracking()
        .Include(attendee => attendee.Badge)
        .OrderBy(attendee => attendee.Id)
        .ToList();

    foreach (var attendee in attendees)
    {
        Console.WriteLine($"Attendee: {attendee.FullName}");
        Console.WriteLine($"Email: {attendee.Email}");
        Console.WriteLine($"Address: {attendee.FullAddress}");
        Console.WriteLine($"Badge Number: {attendee.Badge?.BadgeNumber ?? "No badge"}");

        if (attendee.Badge is not null)
        {
            Console.WriteLine($"Badge Tier: {attendee.Badge.Tier}");
            Console.WriteLine($"Issue Date: {attendee.Badge.IssueDate:yyyy-MM-dd}");
        }

        Console.WriteLine();
    }

    Console.WriteLine("===== Database Counts =====");
    Console.WriteLine($"Organizers: {context.Organizers.Count()}");
    Console.WriteLine($"Organizer Profiles: {context.OrganizerProfiles.Count()}");
    Console.WriteLine($"Events: {context.Events.Count()}");
    Console.WriteLine($"Attendees: {context.Attendees.Count()}");
    Console.WriteLine($"Badges: {context.Badges.Count()}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}