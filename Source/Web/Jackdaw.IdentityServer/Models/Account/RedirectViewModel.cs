﻿namespace Jackdaw.IdentityServer.Models.Account
{
    /// <summary>
    /// Redirect View Model
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
    public class RedirectViewModel
    {
        /// <value>string</value>
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
