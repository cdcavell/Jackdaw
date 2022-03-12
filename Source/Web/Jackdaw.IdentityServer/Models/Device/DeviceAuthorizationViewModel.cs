using Jackdaw.IdentityServer.Models.Consent;

namespace Jackdaw.IdentityServer.Models.Device
{
    /// <summary>
    /// Device Authorization View Model
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
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        /// <value>string</value>
        public string? UserCode { get; set; }
        /// <value>bool</value>
        public bool ConfirmUserCode { get; set; }
    }
}
