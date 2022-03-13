﻿using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using Jackdaw.ClassLibrary.Mvc.Localization;
using Jackdaw.ClassLibrary.Mvc.Services.AppSettings;
using Jackdaw.IdentityServer.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Jackdaw.IdentityServer.Controllers
{
    /// <summary>
    /// External Controller
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
    [AllowAnonymous]
    public class ExternalController : ApplicationBaseController<ExternalController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <param name="signInManager">SignInManager&lt;ApplicationUser&gt;</param>
        /// <param name="interaction">IIdentityServerInteractionService</param>
        /// <param name="clientStore">IClientStore</param>
        /// <param name="events">IEventService</param>
        /// <method>
        /// public ExternalController(
        ///     ILogger&lt;ExternalController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;ExternalController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
        ///     UserManager&lt;ApplicationUser&gt; userManager,
        ///     SignInManager&lt;ApplicationUser&gt; signInManager,
        ///     IIdentityServerInteractionService interaction,
        ///     IClientStore clientStore,
        ///     IEventService events
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService)
        /// </method>
        public ExternalController(
            ILogger<ExternalController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<ExternalController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>Challenge(string scheme, string returnUrl)</method>
        /// <exception cref="Exception">invalid return URL</exception>
        [HttpGet]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = _appSettings.HomeRedirect;

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };

            return Challenge(props, scheme);
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>Callback()</method>
        /// <exception cref="Exception">External authentication error</exception>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            ClaimsPrincipal externalUser = result.Principal;

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                IEnumerable<string> externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await AutoProvisionUserAsync(provider, providerUserId, claims);
            }

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            List<Claim> additionalLocalClaims = new();
            AuthenticationProperties localSignInProps = new();
            CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            IdentityServerUser isuser = new(user.SubjectId)
            {
                DisplayName = user.DisplayName,
                IdentityProvider = provider,
                AdditionalClaims = additionalLocalClaims
            };

            await HttpContext.SignInAsync(isuser, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? _appSettings.HomeRedirect;

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.SubjectId, user.DisplayName, true, context?.Client.ClientId));

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(returnUrl);
                }
            }

            return Redirect(returnUrl);
        }

        private async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            ClaimsPrincipal? externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            Claim userIdClaim = externalUser?.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser?.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            List<Claim> claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            string provider = result.Properties?.Items["scheme"] ?? throw new Exception("Unknown scheme");
            string providerUserId = userIdClaim.Value;

            // find external user
            ApplicationUser user = await _userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            List<Claim> filtered = new();

            // user's display name
            string? name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value.Clean() ??
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value.Clean();
            if (name == null)
            {
                string? first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value.Clean() ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value.Clean();
                string? last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value.Clean() ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value.Clean();
                if (first != null && last != null)
                {
                    name = (first + " " + last).Trim();
                }
                else if (first != null)
                {
                    name = first;
                }
                else if (last != null)
                {
                    name = last;
                }
                else
                {
                    throw new Exception("Missing required name claim");
                }
            }
            
            filtered.Add(new Claim(JwtClaimTypes.Name, name));

            // email
            string? email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value.Clean() ??
               claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value.Clean();
            if (email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }
            else
            {
                throw new Exception("Missing required email claim");
            }

            // setup results
            IdentityResult identityResult = new IdentityResult();

            // check to make sure user dose not exist under diffrent external login
            ApplicationUser user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                // no, so create user
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = email.ToUpper(),
                    DisplayName = name
                };

                identityResult = await _userManager.CreateAsync(user);
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

                // get created user
                user = await _userManager.FindByEmailAsync(email);
                if (user == null) throw new Exception($"User not found for {email}");

                if (filtered.Any())
                {
                    identityResult = await _userManager.AddClaimsAsync(user, filtered);
                    if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                }

                identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            }
            else
            {
                // yes, so update existing user claims
                if (filtered.Any())
                {
                    foreach (Claim claim in filtered)
                    {
                        IdentityUserClaim<string>? foundClaim = user.Claims.Where(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value).FirstOrDefault();
                        if (foundClaim == null)
                        {
                            IdentityUserClaim<string> identityUserClaim = new()
                            {
                                UserId = user.Id,
                                ClaimType = claim.Type,
                                ClaimValue = claim.Value
                            };

                            user.Claims.Add(identityUserClaim);
                        }
                    }

                    UserLoginInfo? userLogin = ((List<UserLoginInfo>)await _userManager.GetLoginsAsync(user))
                        .Where(x => x.LoginProvider == provider && x.ProviderKey == providerUserId)
                        .FirstOrDefault();
                    if (userLogin == null)
                    {
                        identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
                        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                    }

                    identityResult = await _userManager.UpdateAsync(user);
                    if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                }
            }

            return user;
        }

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            Claim? sid = externalResult.Principal?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            string? idToken = externalResult.Properties?.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }
    }
}
