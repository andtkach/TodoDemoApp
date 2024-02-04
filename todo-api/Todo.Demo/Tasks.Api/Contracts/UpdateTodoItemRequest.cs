using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.Contracts;

public class UpdateTodoItemRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Day { get; set; } = string.Empty;
}