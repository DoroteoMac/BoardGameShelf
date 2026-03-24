namespace BoardGameShelf.Endpoints;

/// <summary>
/// Defines endpoints for health checking the service.
/// </summary>
public static class HealthEndpoints
{
    /// <summary>
    /// Maps all health-related endpoints to the route group.
    /// </summary>
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/hello", () => "Hello boardgamer!");

        return group;
    }
}
