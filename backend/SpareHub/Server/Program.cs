using Microsoft.EntityFrameworkCore;
using Persistence;
using Service;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddMemoryCache();

var connectionString = string.Format("server={0};port={1};database={2};user={3};password={4}",
    builder.Configuration.GetValue<string>("MYSQL_HOST"),
    builder.Configuration.GetValue<string>("MYSQL_PORT"),
    builder.Configuration.GetValue<string>("MYSQL_DATABASE"),
    builder.Configuration.GetValue<string>("MYSQL_USER"),
    builder.Configuration.GetValue<string>("MYSQL_PASSWORD"));


builder.Services.AddDbContext<SpareHubDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
var app = builder.Build();

app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());


app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
{
    app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
