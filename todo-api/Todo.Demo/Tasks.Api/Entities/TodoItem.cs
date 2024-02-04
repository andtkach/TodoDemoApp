namespace Tasks.Api.Entities;

public sealed class TodoItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Day { get; set; } = string.Empty;
}
