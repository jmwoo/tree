using System;
using lib.Models;
using lib.Services;

namespace test;

public class TestServiceTests
{
	readonly TestService _testService;

	public TestServiceTests()
	{
		_testService = new();
	}

	[Fact]
	public async Task GetTestAsync_IsHelloWorld()
	{
		const string message = "hello test";
		Test test = await _testService.GetTestAsync(message);
		Assert.Equal(message, test.Message);
		Assert.NotEqual(default, test.TestId);
		Assert.NotEqual(default, test.Timestamp);
	}
}
