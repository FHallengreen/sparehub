using Domain.MongoDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using MongoDB.Driver;
using Neo4j.Driver;
using Persistence;
using Repository.Interfaces;
using Repository.MySql;
using Service;
using Service.Agent;
using Service.Interfaces;
using Service.Mapping;
using Service.MySql.Order;
using Service.Supplier;
using Service.Warehouse;

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

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();

builder.Services.AddScoped<IPortVesselService, PortVesselService>();
builder.Services.AddScoped<IVesselService, VesselService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IOrderRepository, OrderMySqlRepository>();
builder.Services.AddScoped<IBoxRepository, BoxMySqlRepository>();
// Register concrete implementations
builder.Services.AddScoped<OrderMySqlService>();
builder.Services.AddScoped<BoxMySqlService>();

// Keep the interface registrations if they're used elsewhere
builder.Services.AddScoped<IOrderService, OrderMySqlService>();
builder.Services.AddScoped<IBoxService, BoxMySqlService>();

// Register IBoxService and IOrderService with a placeholder default

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
            mySqlOptions => mySqlOptions.EnableStringComparisonTranslations()
)
// .EnableSensitiveDataLogging()
// .LogTo(Console.WriteLine, LogLevel.Information)
);


// MongoDB configuration
var mongoConnectionString = builder.Configuration.GetValue<string>("MONGODB_URI");

builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));

builder.Services.AddScoped(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(builder.Configuration.GetValue<string>("MONGO_INITDB_DATABASE"));
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<OrderCollection>("OrderCollection");
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<BoxOrderCollection>("OrderBoxCollection");
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

await app.RunAsync();
