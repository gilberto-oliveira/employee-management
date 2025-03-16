using EmployeeManagement.Api.Features.Create;
using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Interfaces;

namespace EmployeeManagement.Test;

public class EmployeeCreateCommandHandlerTest
{
    [Fact]
    public async Task Handle_EmployeeAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var repository = Substitute.For<IEmployeeRepository>();
        var dbContext = Substitute.For<IDbContext>();
        var handler = new EmployeeCreateCommandHandler(repository, dbContext);
        var command = new EmployeeCreateCommand(
            "John", 
            "Doe", 
            "12345", 
            "john.doe@example.com", 
            new List<string> { "1234567890" }, 
            "password", 
            "Employee", 
            new DateTime(1990, 1, 1),
            Guid.NewGuid()
        );

        repository.ExistsByDocAsync(command.docNumber, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true)); // Simulate that the employee already exists

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_NewEmployee_AddsEmployeeSuccessfully()
    {
        // Arrange
        var repository = Substitute.For<IEmployeeRepository>();
        var dbContext = Substitute.For<IDbContext>();
        var handler = new EmployeeCreateCommandHandler(repository, dbContext);
        var command = new EmployeeCreateCommand(
            "John", 
            "Doe", 
            "12345", 
            "john.doe@example.com", 
            new List<string> { "1234567890" }, 
            "password", 
            "Employee", 
            new DateTime(1990, 1, 1),
            Guid.NewGuid()
        );

        repository.ExistsByDocAsync(command.docNumber, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Simulate that the employee does not exist

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        repository.Received().Add(Arg.Any<Employee>());
    }

    [Fact]
    public async Task Handle_EmployeeAdded_CallsSaveChangesAsync()
    {
        // Arrange
        var repository = Substitute.For<IEmployeeRepository>();
        var dbContext = Substitute.For<IDbContext>();
        var handler = new EmployeeCreateCommandHandler(repository, dbContext);
        var command = new EmployeeCreateCommand(
            "John", 
            "Doe", 
            "12345", 
            "john.doe@example.com", 
            new List<string> { "1234567890" }, 
            "password", 
            "Employee", 
            new DateTime(1990, 1, 1),
            Guid.NewGuid()
        );

        repository.ExistsByDocAsync(command.docNumber, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Simulate that the employee does not exist

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
