using MaximaBackend.Data;
using MaximaBackend.Services;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Commerce API",
        Version = "v1",
        Description = "API para gerenciamento de produtos do e-commerce",
        Contact = new OpenApiContact
        {
            Name = "Victor Borges",
            Email = "victorborgesp8@gmail.com"
        }
    });
});

// Database connection
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

// Services registration
builder.Services.AddScoped<IProdutoInterface, ProdutoService>();
builder.Services.AddScoped<IDepartamentoInterface, DepartamentoService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    DatabaseInitializer.Initialize(dbConnection);
}

app.Run();
