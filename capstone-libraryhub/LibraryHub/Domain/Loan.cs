namespace LibraryHub.Domain;

public record Loan(int Id, int BookId, int MemberId, DateOnly BorrowedOn, DateOnly DueOn, DateOnly? ReturnedOn = null);