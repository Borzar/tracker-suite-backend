using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;
using TrackerSuite.Core.Data;
using TrackerSuite.Core.Repository.TaskRepository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


// Services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(CreateTaskBO).Assembly
    )
);

// Database connection
// Load .env from the solution
var envPath = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "..", "..", ".env")
);
Env.Load(envPath);
Console.WriteLine($"Loading .env from: {envPath}");
Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"DB_PORT: {Environment.GetEnvironmentVariable("DB_PORT")}");
Console.WriteLine($"DB_NAME: {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"DB_USER: {Environment.GetEnvironmentVariable("DB_USER")}");
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString =     
    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Dependency injection
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Usa el nombre de la política que definiste

app.UseAuthorization();

app.MapControllers();

app.Run();
