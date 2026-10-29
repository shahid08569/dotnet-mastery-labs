namespace LibraryHub.Domain;

public static class Fines
{
    public static decimal Calculate(int daysLate, MemberType type) => (type, daysLate) switch
    {
        (_, <= 0) => 0m,                                  // der hui hi nahi
        (MemberType.Premium, _) => 0m,                                  // Premium: kabhi fine nahi
        (MemberType.Regular, <= 7) => 10m * daysLate,
        (MemberType.Regular, _) => 10m * 7 + 20m * (daysLate - 7),      // 7 din se zyada
        (MemberType.Student, <= 7) => 5m * daysLate,
        (MemberType.Student, _) => 5m * 7 + 10m * (daysLate - 7),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown member type")
    };
}