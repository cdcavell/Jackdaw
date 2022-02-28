using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Filters;
using Jackdaw.IdentityServer.Models.AppSettings;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Serilog;
using System.Globalization;

namespace Jackdaw.IdentityServer
{
    /// <summary>
    /// The HostingExtensions internal static class configures services and the application's request pipeline&lt;br /&gt;&lt;br /&gt;
    /// _Services_ are components that are used by the app. For example, a logging component is a service. Code to configure (_or register_) services are added to the ```ConfigureServices``` method.&lt;br /&gt;&lt;br /&gt;
    /// The request handling pipeline is composed as a series of _middleware_ components. For example, a middleware might handle requests for static files or redirect HTTP requests to HTTPS. Each middleware performs asynchronous operations on an ```HttpContext``` and then either invokes the next middleware in the pipeline or terminates the request. Code to configure the request handling pipeline is added to the ```ConfigurePipeline``` method.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 02/27/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    internal static class HostingExtensions
    {
        private static AppSettings? _appSettings;

        /// <summary>
        /// Method used to add services to the container.
        /// </summary>
        /// <param name="builder">WebApplicationBuilder</param>
        /// <returns>WebApplication</returns>
        /// <method>ConfigureServices(this WebApplicationBuilder builder)</method>
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration));

            _appSettings = new(builder.Configuration);
            builder.Services.AddAppSettingsService(options =>
            {
                options.AppSettings = _appSettings;
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
            if (_appSettings.IsProduction)
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

            builder.Services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(TestUsers.Users);

            return builder.Build();
        }

        /// <summary>
        /// Method used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">WebApplication</param>
        /// <returns>WebApplication</returns>
        /// <method>ConfigurePipeline(this WebApplication app)</method>
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseExceptionHandler("/Home/Error/500");
            app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

            // Add HSTS and HTTPS Redirect to pipeline
            if (_appSettings != null && _appSettings.IsProduction)
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
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            return app;
        }
    }
}
