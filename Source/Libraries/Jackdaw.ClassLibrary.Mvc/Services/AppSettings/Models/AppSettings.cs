using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Jackdaw.ClassLibrary.Mvc.Services.AppSettings.Models
{
    /// <summary>
    /// Application settings information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/12/2021 | Initial Development |~ 
    /// </revision>
    public abstract class AppSettings
    {
        private static readonly string _assemblyName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? String.Empty;
        private static readonly string _assemblyVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? String.Empty;
        private static readonly string _assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? String.Empty;
        private static readonly string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? String.Empty;
        private static readonly DateTime _lastModifiedDateTime = String.IsNullOrEmpty(_assemblyLocation) ? DateTime.MinValue : File.GetLastWriteTime(_assemblyLocation);

        /// <summary>
        /// Contstructor 
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        protected AppSettings(IConfiguration configuration)
        {
            if (configuration != null)
                configuration.Bind("AppSettings", this);
        }

        /// <value>string</value>
        public string AssemblyName { get; } = _assemblyName;

        /// <value>string</value>
        public string AssemblyVersion { get; } = _assemblyVersion;

        /// <value>string</value>
        public string EnvironmentName { get; } = _environmentName;

        /// <value>bool</value>
        public bool IsDevelopment { get; } = _environmentName.Equals("Development", StringComparison.OrdinalIgnoreCase);

        /// <value>bool</value>
        public bool IsProduction { get; } = _environmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);

        /// <value>DateTime</value>
        public DateTime LastModifiedDateTime { get; } = _lastModifiedDateTime;
    }
}
