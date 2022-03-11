using System.ComponentModel.DataAnnotations;

namespace Jackdaw.IdentityServer.Models.Account
{
    /// <summary>
    /// Login Input Model
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
    public class LoginInputModel
    {
        /// <value>string</value>
        [Required]
        public string Username { get; set; } = string.Empty;
        /// <value>string</value>
        [Required]
        public string Password { get; set; } = string.Empty;
        /// <value>bool</value>
        public bool RememberLogin { get; set; }
        /// <value>string</value>
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
