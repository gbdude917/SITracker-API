using Microsoft.EntityFrameworkCore;  
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SITracker.Data;
using SITracker.Interfaces;
using SITracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAdversaryService, AdversaryService>();
builder.Services.AddScoped<ISpiritService, SpiritService>();

// Database Connection
builder.Services.AddDbContext<TrackerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
