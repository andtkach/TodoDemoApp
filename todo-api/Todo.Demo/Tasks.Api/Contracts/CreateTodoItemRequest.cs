using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.Contracts;

public class CreateTodoItemRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Day { get; set; } = string.Empty;
}