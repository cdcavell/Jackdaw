namespace Jackdaw.IdentityServer.Models.Account
{
    /// <summary>
    /// Logged Out View Model
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
    public class LoggedOutViewModel
    {
        /// <value>string</value>
        public string PostLogoutRedirectUri { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientName { get; set; } = string.Empty;
        /// <value>string</value>
        public string SignOutIframeUrl { get; set; } = string.Empty;

        /// <value>bool</value>
        public bool AutomaticRedirectAfterSignOut { get; set; }

        /// <value>string</value>
        public string LogoutId { get; set; } = string.Empty;
        /// <value>bool</value>
        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
        /// <value>string</value>
        public string ExternalAuthenticationScheme { get; set; } = string.Empty;
    }
}
