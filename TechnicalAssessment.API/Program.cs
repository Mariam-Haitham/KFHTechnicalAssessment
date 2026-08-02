using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;
using TechnicalAssessment.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS for Blazor WASM client.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7149", "http://localhost:5086")
              .AllowAnyMethod()
              .AllowAnyHeader()));

// Register the database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=TechnicalAssessment.db"));
    
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed test data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Customers.Any())
    {
        var customers = new List<Customer>
        {
            new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = new DateOnly(1990, 5, 15), KYCStatus = KYCStatus.Verified },
            new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", DateOfBirth = new DateOnly(1988, 3, 22), KYCStatus = KYCStatus.Verified },
            new Customer { FirstName = "Michael", LastName = "Johnson", Email = "michael.j@example.com", DateOfBirth = new DateOnly(1992, 7, 10), KYCStatus = KYCStatus.Pending },
            new Customer { FirstName = "Sarah", LastName = "Williams", Email = "sarah.w@example.com", DateOfBirth = new DateOnly(1985, 11, 8), KYCStatus = KYCStatus.Rejected },
            new Customer { FirstName = "Robert", LastName = "Brown", Email = "robert.b@example.com", DateOfBirth = new DateOnly(1995, 2, 14), KYCStatus = KYCStatus.Pending }
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();

        // Seed accounts for each customer.
        var accounts = new List<Account>();

        // John Doe - 3 accounts
        accounts.AddRange(new[]
        {
            new Account { CustomerId = customers[0].CustomerId, AccountNumber = "1001001", AccountType = AccountType.Savings, Balance = 5000 },
            new Account { CustomerId = customers[0].CustomerId, AccountNumber = "1001002", AccountType = AccountType.Checking, Balance = 2000 },
            new Account { CustomerId = customers[0].CustomerId, AccountNumber = "1001003", AccountType = AccountType.Investment, Balance = 12000 }
        });

        // Jane Smith - 3 accounts
        accounts.AddRange(new[]
        {
            new Account { CustomerId = customers[1].CustomerId, AccountNumber = "1002001", AccountType = AccountType.Savings, Balance = 8500 },
            new Account { CustomerId = customers[1].CustomerId, AccountNumber = "1002002", AccountType = AccountType.Checking, Balance = 3500 },
            new Account { CustomerId = customers[1].CustomerId, AccountNumber = "1002003", AccountType = AccountType.Investment, Balance = 25000 }
        });

        // Michael Johnson - 2 accounts
        accounts.AddRange(new[]
        {
            new Account { CustomerId = customers[2].CustomerId, AccountNumber = "1003001", AccountType = AccountType.Savings, Balance = 2500 },
            new Account { CustomerId = customers[2].CustomerId, AccountNumber = "1003002", AccountType = AccountType.Checking, Balance = 1200 }
        });

        // Sarah Williams - 2 accounts
        accounts.AddRange(new[]
        {
            new Account { CustomerId = customers[3].CustomerId, AccountNumber = "1004001", AccountType = AccountType.Savings, Balance = 6000 },
            new Account { CustomerId = customers[3].CustomerId, AccountNumber = "1004002", AccountType = AccountType.Investment, Balance = 18500 }
        });

        // Robert Brown - 3 accounts
        accounts.AddRange(new[]
        {
            new Account { CustomerId = customers[4].CustomerId, AccountNumber = "1005001", AccountType = AccountType.Checking, Balance = 1800 },
            new Account { CustomerId = customers[4].CustomerId, AccountNumber = "1005002", AccountType = AccountType.Savings, Balance = 7200 },
            new Account { CustomerId = customers[4].CustomerId, AccountNumber = "1005003", AccountType = AccountType.Investment, Balance = 9500 }
        });

        context.Accounts.AddRange(accounts);
        context.SaveChanges();

        // Seed sample transactions.
        var transactions = new List<Transaction>
        {
            // John's accounts
            new Transaction { FromAccountId = accounts[0].AccountId, ToAccountId = accounts[1].AccountId, Amount = 500, TransactionType = TransactionType.Transfer, Status = TransactionStatus.Completed },
            new Transaction { FromAccountId = accounts[1].AccountId, ToAccountId = accounts[2].AccountId, Amount = 1000, TransactionType = TransactionType.Transfer, Status = TransactionStatus.Completed },

            // Jane's accounts
            new Transaction { FromAccountId = accounts[3].AccountId, ToAccountId = accounts[4].AccountId, Amount = 2000, TransactionType = TransactionType.Transfer, Status = TransactionStatus.Completed }
        };
        context.Transactions.AddRange(transactions);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
