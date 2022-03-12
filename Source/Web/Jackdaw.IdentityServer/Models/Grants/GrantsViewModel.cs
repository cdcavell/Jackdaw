namespace Jackdaw.IdentityServer.Models.Grants
{
    /// <summary>
    /// Grants View Model
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
    public class GrantsViewModel
    {
        /// <value>IEnumerable&lt;GrantViewModel&gt;</value>
        public IEnumerable<GrantViewModel> Grants { get; set; } = new List<GrantViewModel>();
    }
}
