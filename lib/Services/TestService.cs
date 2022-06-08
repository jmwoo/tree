using lib.Models;

namespace lib.Services;

public interface ITestService
{
	Task<Test> GetTestAsync(string? message = null);
}

public class TestService : ITestService
{
	private int _id;

	public TestService()
	{
		_id = 0;
	}

	public async Task<Test> GetTestAsync(string? message)
	{
		await Task.Yield();
		return new(++_id, message ?? "hello world", DateTime.UtcNow);
	}
}

