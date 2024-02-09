namespace Tasks.Api.Endpoints;

public static class InfoEndpoints
{
    public static void MapInfoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("info",  (
            ILoggerFactory loggerFactory,
            CancellationToken ct) =>
        {
            var logger = loggerFactory.CreateLogger("Info");
            logger.LogInformation("Info called");
            return Results.Ok(new { api = "todo-api", version = "0.1"});
        });

        
    }
}
