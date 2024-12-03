using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Neo4j.Driver;
using Persistence.MongoDb;
using Persistence.MySql.SparehubDbContext;
using Repository;
using Repository.Interfaces;
using Repository.MongoDb;
using Repository.MySql;
using Server.Middleware;
using Service;
using Service.Interfaces;
using Service.Mapping;
using Service.MySql.Dispatch;
using Service.MongoDb;
using Service.MySql.Agent;
using Service.MySql.Login;
using Service.MySql.Order;
using Service.MySql.Owner;
using Service.MySql.Port;
using Service.MySql.Vessel;
using Service.MySql.VesselAtPort;
using Service.MySql.Supplier;
using Service.MySql.Warehouse;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("AzureLogging"));

// Configure logging providers
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Enables detailed logging in docker container
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Configuration.AddUserSecrets<Program>();

// Add memory caching
builder.Services.AddMemoryCache();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

// Add the DatabaseFactory with IOptionsMonitor
builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();

builder.Services.AddScoped<IDispatchRepository>(sp =>
{
    var databaseFactory = sp.GetRequiredService<IDatabaseFactory>();
    return databaseFactory.GetRepository<IDispatchRepository>();
});

builder.Services.AddScoped<IBoxRepository>(sp =>
{
    var databaseFactory = sp.GetRequiredService<IDatabaseFactory>();
    return databaseFactory.GetRepository<IBoxRepository>();
});

builder.Services.AddScoped<IOrderRepository>(sp =>
{
    var databaseFactory = sp.GetRequiredService<IDatabaseFactory>();
    return databaseFactory.GetRepository<IOrderRepository>();
});

// Register services directly
builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVesselService, VesselService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


// Register MySQL repositories
builder.Services.AddScoped<BoxMySqlRepository>();
builder.Services.AddScoped<PortMySqlRepository>();
builder.Services.AddScoped<VesselMySqlRepository>();
builder.Services.AddScoped<VesselAtPortMySqlRepository>();
builder.Services.AddScoped<BoxMongoDbRepository>();
builder.Services.AddScoped<OrderMySqlRepository>();
builder.Services.AddScoped<DispatchMySqlRepository>();

// Register MongoDB repositories
builder.Services.AddScoped<BoxMongoDbRepository>();
builder.Services.AddScoped<OrderMongoDbRepository>();
builder.Services.AddScoped<DispatchMongoDbRepository>();
builder.Services.AddScoped<OwnerMySqlRepository>();

// Register the services as concrete types
builder.Services.AddScoped<BoxService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PortService>();
builder.Services.AddScoped<VesselService>();
builder.Services.AddScoped<VesselAtPortService>();
builder.Services.AddScoped<DispatchService>();
builder.Services.AddScoped<DispatchMongoDbService>();
builder.Services.AddScoped<OwnerService>();

// Dynamically resolve services using the factory
builder.Services.AddScoped<IBoxService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetRepository<IBoxService>());
builder.Services.AddScoped<IOrderService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetRepository<IOrderService>());
builder.Services.AddScoped<IDispatchService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetRepository<IDispatchService>());
builder.Services.AddScoped<IPortService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetRepository<IPortService>());
builder.Services.AddScoped<IVesselService>(sp =>
    sp.GetRequiredService<IDatabaseFactory>().GetRepository<IVesselService>());

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
    return database.GetCollection<OrderCollection>("orders");
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<BoxCollection>("boxes");
});

builder.Services.AddScoped(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<DispatchCollection>("dispatches");
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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JWT_ISSUER"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT_SECRET_KEY")!)),
            RoleClaimType = ClaimTypes.Role
        };
    });


var app = builder.Build();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

await app.RunAsync();

public partial class Program
{
}