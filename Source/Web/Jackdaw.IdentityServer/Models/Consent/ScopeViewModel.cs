namespace Jackdaw.IdentityServer.Models.Consent
{
    /// <summary>
    /// Scope View Model
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
    public class ScopeViewModel
    {
        /// <value>string</value>
        public string? Value { get; set; }
        /// <value>string</value>
        public string? DisplayName { get; set; }
        /// <value>string</value>
        public string? Description { get; set; }
        /// <value>bool</value>
        public bool Emphasize { get; set; }
        /// <value>bool</value>
        public bool Required { get; set; }
        /// <value>bool</value>
        public bool Checked { get; set; }
    }
}
