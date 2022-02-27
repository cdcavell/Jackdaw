using Jackdaw.Public;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting Host");

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Adding services to the container");
    var app = builder.ConfigureServices();

    Log.Information("Configuring the HTTP request pipeline");
    app.ConfigurePipeline();

    Log.Information("Starting application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
