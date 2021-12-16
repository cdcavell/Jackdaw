using Jackdaw.ClassLibrary.Mvc.Controllers;
using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.Public.Filters;
using Jackdaw.Public.Models.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Jackdaw.Public.Controllers
{
    /// <class>ApplicationBaseController</class>
    /// <summary>
    /// Base controller class for application
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/15/2020 | Initial Development |~ 
    /// </revision>
    [ServiceFilter(typeof(SecurityHeadersAttribute))]
    public abstract partial class ApplicationBaseController<T> : WebBaseController<ApplicationBaseController<T>> where T : ApplicationBaseController<T>
    {
        /// <value>AppSettings</value>
        protected readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>
        /// ApplicationBaseController(
        ///     ILogger&lt;T&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;T&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// )
        /// </method>
        protected ApplicationBaseController(
            ILogger<T> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<T> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
        ) : base(logger, webHostEnvironment, httpContextAccessor, localizer, sharedLocalizer)
        {
            _appSettings = appSettingsService.ToObject<AppSettings>();
        }
    }
}
