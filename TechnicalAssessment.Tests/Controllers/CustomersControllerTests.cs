using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Controllers;
using TechnicalAssessment.API.Data;
using TechnicalAssessment.API.Models;

namespace TechnicalAssessment.Tests.Controllers;

public class CustomersControllerTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private void SeedTestData(AppDbContext context)
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateOnly(1990, 5, 15),
                KYCStatus = KYCStatus.Verified
            },
            new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                DateOfBirth = new DateOnly(1985, 3, 20),
                KYCStatus = KYCStatus.Pending
            },
            new Customer
            {
                FirstName = "Robert",
                LastName = "Johnson",
                Email = "robert.johnson@example.com",
                DateOfBirth = new DateOnly(1995, 8, 10),
                KYCStatus = KYCStatus.Rejected
            }
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();
    }

    [Fact]
    public async Task GetCustomers_ReturnsAllCustomers_WhenNoSearchProvided()
    {
        // Arrange
        var context = CreateInMemoryContext();
        SeedTestData(context);
        var controller = new CustomersController(context);

        // Act
        var result = await controller.GetCustomers(null);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var customers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal(3, customers.Count);
    }

    [Fact]
    public async Task GetCustomers_ReturnsMatchingCustomers_WhenSearchByFirstName()
    {
        // Arrange
        var context = CreateInMemoryContext();
        SeedTestData(context);
        var controller = new CustomersController(context);

        // Act
        var result = await controller.GetCustomers("Jane");

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var customers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Single(customers);
        Assert.Equal("Jane", customers[0].FirstName);
    }

    [Fact]
    public async Task GetCustomers_ReturnsMatchingCustomers_WhenSearchByLastName()
    {
        // Arrange
        var context = CreateInMemoryContext();
        SeedTestData(context);
        var controller = new CustomersController(context);

        // Act
        var result = await controller.GetCustomers("Smith");

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var customers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Single(customers);
        Assert.Equal("Jane", customers[0].FirstName);
    }

    [Fact]
    public async Task GetCustomers_SearchIsCaseInsensitive()
    {
        // Arrange
        var context = CreateInMemoryContext();
        SeedTestData(context);
        var controller = new CustomersController(context);

        // Act — search for "jane" in various cases, should return 1 customer regardless.
        var resultLower = await controller.GetCustomers("jane");
        var resultUpper = await controller.GetCustomers("JANE");
        var resultMixed = await controller.GetCustomers("JaNe");

        // Assert
        var okLower = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(resultLower);
        var customersLower = Assert.IsType<List<Customer>>(okLower.Value);

        var okUpper = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(resultUpper);
        var customersUpper = Assert.IsType<List<Customer>>(okUpper.Value);

        var okMixed = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(resultMixed);
        var customersMixed = Assert.IsType<List<Customer>>(okMixed.Value);

        Assert.Single(customersLower);
        Assert.Single(customersUpper);
        Assert.Single(customersMixed);
    }

    [Fact]
    public async Task GetCustomers_ReturnsEmpty_WhenSearchHasNoMatches()
    {
        // Arrange
        var context = CreateInMemoryContext();
        SeedTestData(context);
        var controller = new CustomersController(context);

        // Act
        var result = await controller.GetCustomers("NonExistentName");

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var customers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Empty(customers);
    }

    [Fact]
    public async Task GetCustomers_ReturnsEmpty_WhenDatabaseIsEmpty()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var controller = new CustomersController(context);

        // Act
        var result = await controller.GetCustomers(null);

        // Assert
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        var customers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Empty(customers);
    }
}
