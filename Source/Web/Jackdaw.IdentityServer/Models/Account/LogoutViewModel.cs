namespace Jackdaw.IdentityServer.Models.Account
{
    /// <summary>
    /// Logout View Model
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
    public class LogoutViewModel : LogoutInputModel
    {
        /// <value>bool</value>
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
