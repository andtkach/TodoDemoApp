using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Tasks.Api.Contracts;
using Tasks.Api.Database;
using Tasks.Api.Entities;

namespace Tasks.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("tasks", async (
            CreateTodoItemRequest request,
            ApplicationDbContext context,
            IMapper mapper,
            CancellationToken ct) =>
        {
            var item = mapper.Map<TodoItem>(request);
            context.Add(item);

            await context.SaveChangesAsync(ct);

            return Results.Ok(item);
        });

        app.MapGet("tasks", async (
            ApplicationDbContext context,
            CancellationToken ct,
            IDistributedCache cache,
            int page = 1,
            int pageSize = 10) =>
        {
            var items = await cache.GetAsync($"{Constants.CacheTasksKey}-{page}-{pageSize}",
                async token =>
                {
                    var items = await context.TodoItems
                        .AsNoTracking()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(token);
                    return items;
                },
                CacheOptions.DefaultExpiration,
                ct);
            
            return Results.Ok(items);
        });

        app.MapGet("tasks/{id}", async (
            int id,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {
            var item = await cache.GetAsync($"{Constants.CacheTaskKey}-{id}",
                async token =>
                {
                    var item = await context.TodoItems
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id, token);

                    return item;
                },
                CacheOptions.DefaultExpiration,
                ct);

            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        app.MapPut("tasks/{id}", async (
            int id,
            UpdateTodoItemRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {
            var item = await context.TodoItems
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (item is null)
            {
                return Results.NotFound();
            }

            item.Name = request.Name;
            item.Day = request.Day;

            await context.SaveChangesAsync(ct);

            await cache.RemoveAsync($"{Constants.CacheTaskKey}-{id}", ct);

            return Results.NoContent();
        });

        app.MapDelete("tasks/{id}", async (
            int id,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {
            var item = await context.TodoItems
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (item is null)
            {
                return Results.NotFound();
            }

            context.Remove(item);

            await context.SaveChangesAsync(ct);
            await cache.RemoveAsync($"{Constants.CacheTaskKey}-{id}", ct);

            return Results.NoContent();
        });
    }
}
