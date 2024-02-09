namespace Tasks.Api.Entities;

public sealed class TodoItem: AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Day { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;
    public string Audio { get; set; } = string.Empty;
}
