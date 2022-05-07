using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jackdaw.ClassLibrary.Data.Models
{
    /// <summary>
    /// CDCavell AuditEntry record
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 05/07/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class AuditEntry
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="entry">EntityEntry</param>
        /// <method>AuditEntry(EntityEntry entry)</method>
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        /// <value>EntityEntry</value>
        public EntityEntry Entry { get; }
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string TableName { get; set; } = String.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string State { get; set; } = String.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string Application { get; set; } = String.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string ModifiedBy { get; set; } = String.Empty;
        /// <value>DateTime</value>
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
        /// <value>Dictionary&lt;string, object&gt;</value>
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        /// <value>Dictionary&lt;string, object&gt;</value>
        public Dictionary<string, object> OriginalValues { get; } = new Dictionary<string, object>();
        /// <value>Dictionary&lt;string, object&gt;</value>
        public Dictionary<string, object> CurrentValues { get; } = new Dictionary<string, object>();
        /// <value>List&lt;PropertyEntry&gt;</value>
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        /// <value>bool</value>
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        /// <summary>
        /// Audit History record to write
        /// </summary>
        /// <method>ToAuditHistory()</method>
        public AuditHistory ToAuditHistory()
        {
            string file = this.GetType().Assembly.Location;
            string app = System.IO.Path.GetFileNameWithoutExtension(file);

            var auditHistory = new AuditHistory
            {
                Entity = TableName,
                State = State,
                Application = string.IsNullOrEmpty(Application) ? app : Application,
                ModifiedBy = ModifiedBy,
                ModifiedOn = ModifiedOn,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OriginalValues = OriginalValues.Count == 0 ? string.Empty : JsonConvert.SerializeObject(OriginalValues),
                CurrentValues = CurrentValues.Count == 0 ? string.Empty : JsonConvert.SerializeObject(CurrentValues)
            };
            return auditHistory;
        }
    }
}
