using NUnit.Framework;
using SudokuConsole.Application.Commands;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class QuitCommandTests
{
    [Test]
    public void Execute_SetsQuitFlag()
    {
        // Arrange
        var command = new QuitCommand();

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.ShouldQuit, Is.True);
        Assert.That(result.ShouldRedisplayGrid, Is.False);
    }

    [Test]
    public void Execute_ReturnsGoodbyeMessage()
    {
        // Arrange
        var command = new QuitCommand();

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Message, Does.Contain("Goodbye").Or.Contains("quit").IgnoreCase);
    }
}
