namespace Todo.Process.Consumers;

public class InputModel
{
    public string Model { get; set; } = "tts-1-hd";
    public string Input { get; set; } = string.Empty;
    public string Voice { get; set; } = "nova";
    public decimal Speed { get; set; } = 1;
}