namespace DiagnosticsWebApi.Entities;

public record UserProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUri { get; set; }
    public string? AvatarBase64 { get; set; }
    public string Bio { get; set; } = string.Empty;
}
