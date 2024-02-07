using Microsoft.EntityFrameworkCore;
using Tasks.Api.Entities;
using Tasks.Api.Services;

namespace Tasks.Api.Database;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ILoggedInUserService? _loggedInUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggedInUserService loggedInUserService)
        : base(options)
    {
        _loggedInUserService = loggedInUserService;
    }

    public DbSet<TodoItem> TodoItems { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _loggedInUserService?.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _loggedInUserService?.UserId;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

}