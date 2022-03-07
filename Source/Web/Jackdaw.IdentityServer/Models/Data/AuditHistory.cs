using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Jackdaw.IdentityServer.Models.Data
{
    /// <summary>
    /// Authorization Service AuditHistory Entity
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    [Table("AuditHistory")]
    public class AuditHistory
    {
        /// <value>long</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string ModifiedBy { get; set; } = string.Empty;
        /// <value>DateTime?</value>
        [AllowNull]
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; } = DateTime.MinValue;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string Application { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string Entity { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string State { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string KeyValues { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string OriginalValues { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string CurrentValues { get; set; } = string.Empty;
    }
}
