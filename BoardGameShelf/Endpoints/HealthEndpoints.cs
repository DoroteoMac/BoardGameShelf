namespace BoardGameShelf.Endpoints;

public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/hello", () => "Hello boardgamer!");

        return group;
    }
}
