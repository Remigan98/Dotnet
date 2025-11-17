namespace ToDos.MinimalAPI.ToDo;

public class ToDo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
}
