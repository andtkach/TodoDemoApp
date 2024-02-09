using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Channels;
using Tasks.Api.Contracts;
using Tasks.Api.Database;
using Tasks.Api.Entities;
using Tasks.Api.Services;
using Tasks.Common.Message;

namespace Tasks.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("tasks", async (
            CreateTodoItemRequest request,
            ApplicationDbContext context,
            IMapper mapper,
            ILoggedInUserService loggedInUserService,
            Channel<MessageTask> channel,
            CancellationToken ct) =>
        {
            var item = mapper.Map<TodoItem>(request);
            item.Owner = loggedInUserService.UserId;
            context.Add(item);
            await context.SaveChangesAsync(ct);
            var message = new MessageTask { Id = item.Id };
            await channel.Writer.WriteAsync(message, ct);
            return Results.Ok(item);
        })
        .RequireAuthorization();

        app.MapGet("tasks", async (
            ApplicationDbContext context,
            ILoggedInUserService loggedInUserService,
            CancellationToken ct,
            IDistributedCache cache,
            int page = 1,
            int pageSize = 10) =>
        {
            var userId = loggedInUserService.UserId;
            var items = await cache.GetAsync($"{Constants.CacheTasksKey}-{userId}-{page}-{pageSize}",
                async token =>
                {
                    var items = await context.TodoItems
                        .Where(p => p.Owner == userId)
                        .AsNoTracking()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(token);
                    return items;
                },
                CacheOptions.DefaultExpiration,
                ct);
            
            return Results.Ok(items);
        })
        .RequireAuthorization();

        app.MapGet("tasks/{id}", async (
            int id,
            ApplicationDbContext context,
            ILoggedInUserService loggedInUserService,
            IDistributedCache cache,
            CancellationToken ct) =>
        {
            var userId = loggedInUserService.UserId;
            var item = await cache.GetAsync($"{Constants.CacheTaskKey}-{userId}-{id}",
                async token =>
                {
                    var item = await context.TodoItems
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id && p.Owner == userId, token);

                    return item;
                },
                CacheOptions.DefaultExpiration,
                ct);

            return item is null ? Results.NotFound() : Results.Ok(item);
        })
        .RequireAuthorization();

        app.MapPut("tasks/{id}", async (
            int id,
            UpdateTodoItemRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            ILoggedInUserService loggedInUserService,
            Channel<MessageTask> channel,
            CancellationToken ct) =>
        {
            var userId = loggedInUserService.UserId;
            var item = await context.TodoItems
                .FirstOrDefaultAsync(p => p.Id == id && p.Owner == userId, ct);

            if (item is null)
            {
                return Results.NotFound();
            }

            item.Name = request.Name;
            item.Day = request.Day;

            await context.SaveChangesAsync(ct);
            
            var message = new MessageTask { Id = item.Id };
            await channel.Writer.WriteAsync(message, ct);

            await cache.RemoveAsync($"{Constants.CacheTaskKey}-{userId}-{id}", ct);

            return Results.NoContent();
        })
        .RequireAuthorization();

        app.MapDelete("tasks/{id}", async (
            int id,
            ApplicationDbContext context,
            IDistributedCache cache,
            ILoggedInUserService loggedInUserService,
            CancellationToken ct) =>
        {
            var userId = loggedInUserService.UserId;
            var item = await context.TodoItems
                .FirstOrDefaultAsync(p => p.Id == id && p.Owner == userId, ct);

            if (item is null)
            {
                return Results.NotFound();
            }

            context.Remove(item);

            await context.SaveChangesAsync(ct);
            await cache.RemoveAsync($"{Constants.CacheTaskKey}-{userId}-{id}", ct);

            return Results.NoContent();
        })
        .RequireAuthorization();
    }
}
