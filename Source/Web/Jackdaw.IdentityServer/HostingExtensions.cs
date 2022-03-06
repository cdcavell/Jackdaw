using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Filters;
using Jackdaw.IdentityServer.Models.AppSettings;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using System.Reflection;

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
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
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

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddIdentityServer()
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlite(_appSettings.ConnectionStrings.EntityFrameworkConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlite(_appSettings.ConnectionStrings.EntityFrameworkConnection,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddTestUsers(TestUsers.Users);

            builder.Services.AddAuthentication()
                .AddGoogle("Google", googleOptions =>
                {
                    googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    googleOptions.ClientId = _appSettings.Authentication.Google.ClientId;
                    googleOptions.ClientSecret = _appSettings.Authentication.Google.ClientSecret;
                });

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

            InitializeDatabase(app);

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

        /// <summary>
        /// InitializeDatabase private method
        /// &lt;br /&gt;
        /// To Initialize: dotnet ef migrations add InitialCreate --context PersistedGrantDbContext
        /// &lt;br /&gt;
        ///                dotnet ef migrations add InitialCreate --context ConfigurationDbContext
        /// &lt;br /&gt;
        /// &lt;br /&gt;
        /// To Update:     dotnet ef migrations add UpdateDatabase_&lt;&lt;YYYY-MM-DD&gt;&gt; -- context PersistedGrantDbContext
        /// &lt;br /&gt;
        ///                dotnet ef migrations add UpdateDatabase_&lt;&lt;YYYY-MM-DD&gt;&gt; -- context ConfigurationDbContext
        /// &lt;br /&gt;&lt;br /&gt;
        /// EF Core tools reference: https://docs.microsoft.com/en-us/ef/core/cli/dotnet
        /// &lt;br /&gt;
        /// Install EF Core Tools: dotnet tool install --global dotnet-ef
        /// &lt;br /&gt;
        /// Upgrade EF Core Tools: dotnet tool update --global dotnet-ef
        /// &lt;br /&gt;&lt;br /&gt;
        /// _Before you can use the tools on a specific project, you'll need to add the `Microsoft.EntityFrameworkCore.Design` package to it._
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <method>InitializeDatabase(IApplicationBuilder app)</method>
        private static void InitializeDatabase(IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateScope())
                {
                    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    if (!context.Clients.Any())
                    {
                        foreach (var client in Config.Clients)
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in Config.IdentityResources)
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var resource in Config.ApiScopes)
                        {
                            context.ApiScopes.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                }
        }
    }
}
