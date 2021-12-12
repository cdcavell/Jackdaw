using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.Public.Filters;
using Jackdaw.Public.Models.AppSettings;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting Host");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

    Log.Information("Adding services to the container");

    builder.Services.AddAppSettingsService(options => 
    { 
        options.AppSettings = new AppSettings(builder.Configuration); 
    });

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllersWithViews();

    builder.Services.AddScoped<SecurityHeadersAttribute>();

    var app = builder.Build();
    Log.Information("Configuring the HTTP request pipeline");

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

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
