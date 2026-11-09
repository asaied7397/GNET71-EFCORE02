using Microsoft.EntityFrameworkCore.Design;

namespace EventHub.Data;

public class EventHubContextFactory
    : IDesignTimeDbContextFactory<EventHubContext>
{
    public EventHubContext CreateDbContext(string[] args)
    {
        string? connectionString =
            Environment.GetEnvironmentVariable(
                "EVENTHUB_CONNECTION_STRING"
            );

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set EVENTHUB_CONNECTION_STRING before using EF Core tools."
            );
        }

        return new EventHubContext(connectionString);
    }
}