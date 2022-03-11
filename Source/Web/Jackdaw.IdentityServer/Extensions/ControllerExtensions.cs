using Jackdaw.IdentityServer.Models.Account;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Controller Extensions
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/11/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Loading page.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>LoadingPage(this Controller controller, string viewName, string redirectUri)</method>
        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }
    }
}
