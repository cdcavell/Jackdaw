﻿namespace Jackdaw.IdentityServer.Models.Account
{
    /// <summary>
    /// External Provider Model
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
    public class ExternalProvider
    {
        /// <value>string</value>
        public string DisplayName { get; set; } = string.Empty;
        /// <value>string</value>
        public string AuthenticationScheme { get; set; } = string.Empty;
    }
}
