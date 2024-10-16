using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using MongoDB.Driver;
using Neo4j.Driver;
using Persistence;
using Server;
using Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("AzureLogging"));

// Configure logging providers
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddUserSecrets<Program>();

// Add memory caching
builder.Services.AddMemoryCache();

// Add OrderService
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IPortVesselService, PortVesselService>();
builder.Services.AddScoped<IVesselService, VesselService>();

// Configure the database connection string with SSL enabled
var connectionString = string.Format("server={0};port={1};database={2};user={3};password={4};SslMode=Required",
    builder.Configuration.GetValue<string>("MYSQL_HOST"),
    builder.Configuration.GetValue<string>("MYSQL_PORT"),
    builder.Configuration.GetValue<string>("MYSQL_DATABASE"),
    builder.Configuration.GetValue<string>("MYSQL_USER"),
    builder.Configuration.GetValue<string>("MYSQL_PASSWORD"));

// Register the DbContext with MySQL configuration
builder.Services.AddDbContext<SpareHubDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            mySqlOptions => mySqlOptions.EnableStringComparisonTranslations())
            /*.EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information)*/ // Enable for debugging
    );


// Mongo DB configuration
var mongoConnectionString = builder.Configuration.GetValue<string>("MONGODB_URI");

Console.WriteLine();
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));

builder.Services.AddScoped(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(builder.Configuration.GetValue<string>("MONGO_INITDB_DATABASE"));
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<OrderBoxCollection>("OrderBoxCollection");
});


// Neo4j configuration
var neo4JUrl = builder.Configuration.GetValue<string>("NEO4J_URL");
var neo4JUsername = builder.Configuration.GetValue<string>("NEO4J_USERNAME");
var neo4JPassword = builder.Configuration.GetValue<string>("NEO4J_PASSWORD");

builder.Services.AddSingleton(GraphDatabase.Driver(
    neo4JUrl,
    AuthTokens.Basic(neo4JUsername, neo4JPassword)
));

// Add Neo4j session service for scoped use
builder.Services.AddScoped(sp =>
{
    var driver = sp.GetRequiredService<IDriver>();
    return driver.AsyncSession();
});

var app = builder.Build();

// Configure CORS policy
app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.WithOrigins(
            "http://localhost:5173",
            "https://sparehub.fhallengreen.com"
        )
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
