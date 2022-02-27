using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Filters;
using Jackdaw.IdentityServer.Models.AppSettings;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Serilog;
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

    AppSettings appSettings = new(builder.Configuration);
    builder.Services.AddAppSettingsService(options =>
    {
        options.AppSettings = appSettings;
    });

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();

    builder.Services.AddSingleton<SharedResource>();
    builder.Services.AddScoped<SecurityHeadersAttribute>();

    // Enable localization
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    // Enable HSTS and HTTPS Redirect
    if (appSettings.IsProduction)
    {
        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(730);
        });

        builder.Services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            options.HttpsPort = 443;
        });
    }

    var app = builder.Build();
    Log.Information("Configuring the HTTP request pipeline");

    app.UseSerilogRequestLogging();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

    // Add HSTS and HTTPS Redirect to pipeline
    if (appSettings.IsProduction)
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    // Localization support
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("nl"),
        new CultureInfo("fr"),
        new CultureInfo("es"),
        new CultureInfo("ja"),
        new CultureInfo("ar"),
        new CultureInfo("uk")
    };
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    app.UseRouting();
    app.UseAuthorization();

    app.UseStaticFiles();
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
