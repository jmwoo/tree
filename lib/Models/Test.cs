namespace lib.Models;

public record Test(int TestId, string Message, DateTime Timestamp)
{
	public string? Environment { get; set; }
}
