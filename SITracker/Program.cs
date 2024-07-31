using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SITracker.Data;
using SITracker.Interfaces;
using SITracker.Models;
using SITracker.Services;
using System.Text;
using System.Security.Claims;

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
            OnTokenValidated = async context =>
            {
                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var lastPasswordChangeClaim = context.Principal.FindFirst("LastPasswordChange")?.Value;

                if (userId != null && lastPasswordChangeClaim != null)
                {
                    var userActionResult = await userService.GetUserById(long.Parse(userId));
                    var user = userActionResult.Value;
                    
                    if (user == null)
                    {
                        context.Fail("User not found.");
                        return;
                    }

                    // Parse the LastPasswordChange claim to DateTime
                    if (DateTime.TryParse(lastPasswordChangeClaim, null, System.Globalization.DateTimeStyles.RoundtripKind, out var lastPasswordChangeDateClaim))
                    {
                        var lastPasswordChangeDateValue = user.LastPasswordChange;
                        
                        if (lastPasswordChangeDateValue != null)
                        {
                            // Parse LastPasswordChangeDateValue from user found in db table
                            var lastPasswordChangeDateValueUtc = DateTime.SpecifyKind(lastPasswordChangeDateValue.Value, DateTimeKind.Utc);

                            // Compare jwt claim and db values
                            if (lastPasswordChangeDateClaim != lastPasswordChangeDateValueUtc)
                            {
                                context.Fail("Token is no longer valid");
                                return;
                            }
                        }

                    }
                }

                Console.WriteLine("Token validated successfully.");
            }
        };
    });
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

    app.MapControllers();
}