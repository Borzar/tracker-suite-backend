using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;
using TrackerSuite.Core.Data;
using TrackerSuite.Core.Repository.TaskRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURATION (ENV)
var envPath = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "..", "..", ".env")
);
Env.Load(envPath);
Console.WriteLine($"Loading .env from: {envPath}");
Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"DB_PORT: {Environment.GetEnvironmentVariable("DB_PORT")}");
Console.WriteLine($"DB_NAME: {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"DB_USER: {Environment.GetEnvironmentVariable("DB_USER")}");
Console.WriteLine($"JWT_ISSUER: {Environment.GetEnvironmentVariable("ISSUER")}");
Console.WriteLine($"JWT_AUDIENCE: {Environment.GetEnvironmentVariable("AUDIENCE")}");


// JWT AUTHENTICATION
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var secretKey = Environment.GetEnvironmentVariable("KEY");
    var issuer = Environment.GetEnvironmentVariable("ISSUER");
    var audience = Environment.GetEnvironmentVariable("AUDIENCE");

    options.TokenValidationParameters = 
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
});

// CONFIGURATION CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// CONFIGURATION MEDIATR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies( typeof(Program).Assembly, typeof(CreateTaskBO).Assembly));

// DATABASE 
var connectionString =     
    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// SERVICES - CONTROLLERS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

// CUSTOM SERVICES (DI)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// PIPELINE MIDDLEWARE
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Usa el nombre de la política que definiste
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
