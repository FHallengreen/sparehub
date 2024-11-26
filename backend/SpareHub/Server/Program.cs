using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using MongoDB.Driver;
using Neo4j.Driver;
using Persistence;
using Persistence.MongoDb;
using Repository.Interfaces;
using Repository.MongoDb;
using Repository.MySql;
using Server.Middleware;
using Service;
using Service.Agent;
using Service.Interfaces;
using Service.Mapping;
using Service.MySql.Dispatch;
using Service.MongoDb;
using Service.MySql.Order;
using Service.MySql.Warehouse;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("AzureLogging"));

// Configure logging providers
/*builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Debug);*/

// Enables detailed logging in docker container
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddUserSecrets<Program>();

// Add memory caching
builder.Services.AddMemoryCache();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

// Add the DatabaseFactory with IOptionsMonitor
builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();

// Register the repositories as concrete types
builder.Services.AddScoped<BoxMySqlRepository>();
builder.Services.AddScoped<OrderMySqlRepository>();
builder.Services.AddScoped<OrderMongoDbRepository>();
builder.Services.AddScoped<DispatchMySqlRepository>();
builder.Services.AddScoped<DispatchMongoDbRepository>();
builder.Services.AddScoped<WarehouseMySqlRepository>();
builder.Services.AddScoped<AddressMySqlRepository>();
builder.Services.AddScoped<AgentMySqlRepository>();

// Register the services as concrete types
builder.Services.AddScoped<BoxMySqlService>();
builder.Services.AddScoped<OrderMySqlService>();
builder.Services.AddScoped<OrderMongoDbService>();
builder.Services.AddScoped<DispatchMySqlService>();
builder.Services.AddScoped<DispatchMongoDbService>();
builder.Services.AddScoped<WarehouseMySqlService>();

// Dynamically resolve services using the factory
builder.Services.AddScoped<IBoxService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetService<IBoxService>());
builder.Services.AddScoped<IOrderService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetService<IOrderService>());
builder.Services.AddScoped<IDispatchService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetService<IDispatchService>());
builder.Services.AddScoped<IWarehouseService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetService<IWarehouseService>());

// Add AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingMySqlProfile>();
    cfg.AddProfile<MappingMongoDbProfile>();
});


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
    return database.GetCollection<OrderCollection>("Order");
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<BoxCollection>("Box");
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<DispatchCollection>("Dispatch");
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

app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

await app.RunAsync();