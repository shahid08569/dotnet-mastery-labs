namespace LibraryHub.Domain;

public class Member
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }                      // optional, null ho sakta hai
    public MemberType Type { get; init; } = MemberType.Regular;
}