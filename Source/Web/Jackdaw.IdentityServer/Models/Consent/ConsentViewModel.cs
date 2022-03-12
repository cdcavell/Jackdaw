namespace Jackdaw.IdentityServer.Models.Consent
{
    /// <summary>
    /// Consent View Model
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
    public class ConsentViewModel : ConsentInputModel
    {
        /// <value>string</value>
        public string? ClientName { get; set; }
        /// <value>string</value>
        public string? ClientUrl { get; set; }
        /// <value>string</value>
        public string? ClientLogoUrl { get; set; }
        /// <value>bool</value>
        public bool AllowRememberConsent { get; set; }

        /// <value>IEnumerable&lt;ScopeViewModel&gt;</value>
        public IEnumerable<ScopeViewModel>? IdentityScopes { get; set; }
        /// <value>IEnumerable&lt;ScopeViewModel&gt;</value>
        public IEnumerable<ScopeViewModel>? ApiScopes { get; set; }
    }
}
