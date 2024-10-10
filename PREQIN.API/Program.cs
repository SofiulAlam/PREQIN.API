using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using PREQIN.API.Data;
using PREQIN.API.Mapping;
using PREQIN.API.Models;
using PREQIN.API.Repositories;
using System;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=investors.db"));

builder.Services.AddScoped<IInvestorRepository, InvestorRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:5173") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowReactApp");

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();


        // Load CSV data into the database
        await LoadDataFromCsvAsync(dbContext, "data.csv");
    }
    catch (Exception ex)
    {
        // Log the error (you can use an ILogger here)
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

async Task LoadDataFromCsvAsync(ApplicationDbContext dbContext, string filePath)
{
    var investors = new List<Investor>();

    using (var reader = new StreamReader(filePath))
    {
        var line = await reader.ReadLineAsync(); // Reads the header line
        while ((line = await reader.ReadLineAsync()) != null)
        {
            var values = line.Split(',');

            // Create a new Investor instance
            var investor = new Investor
            {
                Id = Int32.Parse(values[0]),
                Name = values[1],
                Type = values[2],
                Country = values[3],
                DateAdded = DateTime.ParseExact(values[4], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                LastUpdated = DateTime.ParseExact(values[5], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Commitments = new List<Commitment>
                {
                    new Commitment
                    {
                        AssetClass = values[6],
                        Amount = decimal.Parse(values[7]),
                        Currency = values[8],
                    }
                }
            };
            investors.Add(investor);
        }
    }

    // Add the investors to the database
    await dbContext.Investors.AddRangeAsync(investors);
    await dbContext.SaveChangesAsync();
}

app.Run();
