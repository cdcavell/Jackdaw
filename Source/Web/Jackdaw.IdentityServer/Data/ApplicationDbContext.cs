using Duende.IdentityServer.Models;
using Jackdaw.IdentityServer.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jackdaw.IdentityServer.Data
{
    /// <summary>
    /// Application Database Context
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <seealso cref="IdentityDbContext" />
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly string _clientId;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="options">DbContextOptions&lt;ApplicationDbContext&gt;</param>
        /// <param name="logger">ILogger&lt;ApplicationDbContext&gt;</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <method>
        /// ApplicationDbContext(
        ///     DbContextOptions&lt;ApplicationDbContext&gt; options
        /// ) : base(options)
        /// </method>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ILogger<ApplicationDbContext> logger,
            IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            if (_httpContextAccessor.HttpContext != null)
            {
                var request = _httpContextAccessor.HttpContext.Request;
                var remoteIp = request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                var user = _httpContextAccessor.HttpContext.User;

                Claim? idClaim = (user != null) ? user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("email") : null;
                if (idClaim != null)
                    _userId = idClaim.Value ?? (remoteIp ?? "Anonymous");
                else
                    _userId = remoteIp ?? "Anonymous";

                Claim? clientIdClaim = (user != null) ? user.FindFirst("client_id") : null;
                if (clientIdClaim != null)
                    _clientId = clientIdClaim.Value;
            }
            else
            {
                string file = this.GetType().Assembly.Location;
                string app = System.IO.Path.GetFileNameWithoutExtension(file);
                _userId = app;
                _clientId = app;
            }

            _userId = _userId ?? "Anonymous";
            _clientId = _clientId ?? "Anonymous";
        }

        /// <value>DbSet&lt;AuditHistory&gt;</value>
        public DbSet<AuditHistory> AuditHistory => Set<AuditHistory>();
        /// <value>DbSet&lt;SerializedKey&gt;</value>
        public DbSet<SerializedKey> SerializedKey => Set<SerializedKey>();

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

        /// <summary>
        /// Checks for any unsaved INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <returns>bool</returns>
        /// <method>HasUnsavedChanges()</method>
        public bool HasUnsavedChanges()
        {
            return ChangeTracker.HasChanges();
        }

        /// <summary>
        /// Override to record all the data change history in a table named ```Audit```, this table 
        /// contains INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <returns>int</returns>
        /// <method>SaveChanges(bool acceptAllChangesOnSuccess = true)</method>
        public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            // uncomment to have audit records greated than 7 days old removed from dbo.AuditHistory table
            RemoveAuditRecords();

            List<AuditEntry> auditEntries = OnBeforeSaveChanges(_userId, _clientId);
            int result = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSaveChanges(auditEntries);

            return result;
        }

        /// <summary>
        /// Override to record all the data change history in a table named ```Audit```, this table 
        /// contains INSERT, UPDATE, DELETE history.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task&lt;int&gt;</returns>
        /// <method>SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))</method>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // uncomment to have audit records greated than 7 days old removed from dbo.AuditHistory table
            RemoveAuditRecords();

            List<AuditEntry> auditEntries = OnBeforeSaveChanges(_userId, _clientId);
            int result = await base.SaveChangesAsync(true, cancellationToken);
            await OnAfterSaveChangesAsync(auditEntries);

            return result;
        }

        /// <summary>
        /// Save audit entities that have all the modifications and return list of entries 
        /// where the value of some properties are unknown at this step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <returns>List&lt;AuditEntry&gt;</returns>
        /// <method>OnBeforeSaveChanges(string userId)</method>
        private List<AuditEntry> OnBeforeSaveChanges(string userId, string clientId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditHistory || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.GetTableName() ?? string.Empty;
                auditEntry.Application = clientId;
                auditEntry.ModifiedBy = userId;
                auditEntry.ModifiedOn = DateTime.UtcNow;
                auditEntry.State = entry.State.ToString();
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue ?? string.Empty;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.CurrentValues[propertyName] = property.CurrentValue ?? string.Empty;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OriginalValues[propertyName] = property.OriginalValue ?? string.Empty;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OriginalValues[propertyName] = property.OriginalValue ?? string.Empty;
                                auditEntry.CurrentValues[propertyName] = property.CurrentValue ?? string.Empty;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditHistory.Add(auditEntry.ToAuditHistory());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        /// <summary>
        /// Save audit entities where the value of some properties were unknown at previous step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <method>OnAfterSaveChanges(List&lt;AuditEntry&gt; auditEntries)</method>
        private void OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                    else
                    {
                        auditEntry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                }

                // Save the Audit entry
                AuditHistory.Add(auditEntry.ToAuditHistory());
                SaveChanges();
            }

            return;
        }

        /// <summary>
        /// Save audit entities where the value of some properties were unknown at previous step.
        /// &lt;br/&gt;&lt;br/&gt;
        /// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        /// </summary>
        /// <method>OnAfterSaveChangesAsync(List&lt;AuditEntry&gt; auditEntries)</method>
        private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                    else
                    {
                        auditEntry.CurrentValues[prop.Metadata.Name] = prop.CurrentValue ?? string.Empty;
                    }
                }

                // Save the Audit entry
                AuditHistory.Add(auditEntry.ToAuditHistory());
            }

            return SaveChangesAsync();
        }

        /// <summary>
        /// Remove audit records greater than 7 days old
        /// </summary>
        /// <method>RemoveAuditRecords()</method>
        private void RemoveAuditRecords()
        {
            IQueryable<AuditHistory> query = this.AuditHistory.Where(Audit => Audit.ModifiedOn < DateTime.Now.AddDays(-7));
            if (query.Any())
                this.AuditHistory.RemoveRange(query);
        }
    }
}
