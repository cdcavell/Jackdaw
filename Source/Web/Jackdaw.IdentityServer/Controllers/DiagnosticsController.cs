using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Models.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Jackdaw.IdentityServer.Controllers
{
    /// <summary>
    /// Diagnostics Controller
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/12/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class DiagnosticsController : ApplicationBaseController<DiagnosticsController>
    {
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
        /// public DiagnosticsController(
        ///     ILogger&lt;DiagnosticsController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;DiagnosticsController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService)
        /// </method>
        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<DiagnosticsController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
        }

        /// <summary>
        /// Index method
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>Index()</method>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_appSettings.IsProduction)
                return Redirect(_appSettings.HomeRedirect);

            var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            return View(model);
        }
    }
}
