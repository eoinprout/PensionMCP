using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PensionMCP.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

var connectionString = DbUtils.BuildConnectionString();

builder.Services.AddDbContext<PensionDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("PensionMCP starting.");


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PensionDbContext>();
    var dbPath = DbUtils.GetDatabasePath();
    var dbExists = File.Exists(dbPath);
    await db.Database.EnsureCreatedAsync();
    logger.LogInformation("Database path: {DbPath}", dbPath);
    logger.LogInformation(dbExists ? "Database already existed." : "Database was created.");
}

await app.RunAsync();


