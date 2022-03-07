using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Jackdaw.IdentityServer.Controllers
{
    /// <summary>
    /// Account Controller
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/07/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    [AllowAnonymous]
    public class AccountController : ApplicationBaseController<AccountController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="interaction">IIdentityServerInteractionService</param> 
        /// <param name="clientStore">IClientStore</param>
        /// <param name="schemeProvider">IAuthenticationSchemeProvider</param>
        /// <param name="identityProviderStore">IIdentityProviderStore</param>
        /// <param name="events">IEventService</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <param name="signInManager">SignInManager&lt;ApplicationUser&gt;</param> 
        /// <method>
        /// public AccountController(
        ///     ILogger&lt;AccountController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;AccountController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        ///     IIdentityServerInteractionService interaction,
        ///     IClientStore clientStore,
        ///     IAuthenticationSchemeProvider schemeProvider,
        ///     IIdentityProviderStore identityProviderStore,
        ///     IEventService events,
        ///     UserManager&lt;ApplicationUser&gt; userManager,
        ///     SignInManager&lt;ApplicationUser&gt; signInManager
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService)
        /// </method>
        public AccountController(
            ILogger<AccountController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<AccountController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityProviderStore identityProviderStore,
            IEventService events,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>Login(string returnUrl)</method>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return Redirect("~/");

            return Redirect("~/");
        }
    }
}
