using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.Public.Filters;
using Jackdaw.Public.Models.AppSettings;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Serilog.Events;
using System.Globalization;

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

    // Enable localization
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    var app = builder.Build();
    Log.Information("Configuring the HTTP request pipeline");

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    // Localization support
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("nl"),
        new CultureInfo("fr"),
        new CultureInfo("es"),
        new CultureInfo("ja")
    };
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

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
