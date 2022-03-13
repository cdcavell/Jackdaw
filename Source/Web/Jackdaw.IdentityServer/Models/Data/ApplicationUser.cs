using Microsoft.AspNetCore.Identity;

namespace Jackdaw.IdentityServer.Models.Data
{
    /// <summary>
    /// ApplicationUser Entity
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
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Initializes a user identity
        /// </summary>
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <value>string</value>
        public string SubjectId { get => this.Id; }
        /// <value>string</value>
        public string DisplayName { get; set; } = "Anonymous";
        /// <value>ICollection&lt;IdentityUserClaim&lt;string&gt;&gt;</value>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    }
}
