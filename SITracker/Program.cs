using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SITracker.Data;
using SITracker.Interfaces;
using SITracker.Models;
using SITracker.Services;
using System.Text;
using System.Text.Json;
using SITracker.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Retrieve secrets
builder.Configuration.AddUserSecrets<Program>();

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
Configure(app, app.Environment);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add services to the container.
    services.AddScoped<IAdversaryService, AdversaryService>();
    services.AddScoped<ISpiritService, SpiritService>();
    services.AddScoped<IGameSessionService, GameSessionService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserAuthService, UserService>();
    services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
    services.AddScoped<IJwtService, JwtService>();

    // Database Connection
    services.AddDbContext<TrackerDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

    // Configure strongly typed settings object
    services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

    // Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Add authorization services
    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    services.AddSingleton(tokenValidationParameters);

    // Jwt Authentication Configuration
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };
    });

    //services.AddAuthorization();
}

void Configure(WebApplication app, IWebHostEnvironment env)
{
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();


    // Add custom jwt middleware
    //app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}