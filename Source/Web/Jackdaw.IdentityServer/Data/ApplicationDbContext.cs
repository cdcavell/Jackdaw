using Jackdaw.IdentityServer.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jackdaw.IdentityServer.Data
{
    /// <summary>
    /// Application Database Context
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="options">DbContextOptions&lt;ApplicationDbContext&gt;</param>
        /// <method>ApplicationDbContext(DbContextOptions&lt;ApplicationDbContext&gt; options) : base(options)</method>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating method
        /// &lt;br /&gt;&lt;br /&gt;
        /// Customize the ASP.NET Identity model and override the defaults if needed.
        /// For example, you can rename the ASP.NET Identity table names and more.
        /// Add your customizations after calling base.OnModelCreating(builder);
        /// </summary>
        /// <param name="builder">ModelBuilder</param>
        /// <method>OnModelCreating(ModelBuilder builder)</method>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
