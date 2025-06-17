namespace ConTech.Core;

public class SystemOptions
{
    public string NgoSystemHost { get; set; } = string.Empty;

    public string AdminSystemHost { get; set; } = string.Empty;

    public bool Validate()
    {
        return !string.IsNullOrWhiteSpace(NgoSystemHost) && !string.IsNullOrWhiteSpace(AdminSystemHost);
    }
}