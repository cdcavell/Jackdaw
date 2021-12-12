using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Jackdaw.ClassLibrary.Mvc.Services.AppSettings
{
    /// <summary>
    /// AppSettings Web Service
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/12/2021 | Initial Development |~ 
    /// </revision>
    public class AppSettingsService : IAppSettingsService
    {
        private readonly Models.AppSettings _appSettings;
        private readonly string _rawJson;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">IOptions&lt;AppSettingsServiceOptions&gt;</param>
        /// <method>UserAuthorizationService(IOptions&lt;UserAuthorizationServiceOptions&gt; options)</method>
        public AppSettingsService(IOptions<AppSettingsServiceOptions> options)
        {
            _appSettings = (Models.AppSettings)options.Value.AppSettings;
            _rawJson = JsonConvert.SerializeObject(options.Value.AppSettings);
        }

        /// <summary>
        /// Get AssemblyName value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyName()
        {
            return _appSettings.AssemblyName;
        }

        /// <summary>
        /// Get AssemblyVersion value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyVersion()
        {
            return _appSettings.AssemblyVersion;
        }

        /// <summary>
        /// Get LastModifiedDate value
        /// </summary>
        /// <returns>string</returns>
        public string LastModifiedDate()
        {
            return _appSettings.LastModifiedDateTime.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Get EnvironmentName value
        /// </summary>
        /// <returns>string</returns>
        public string EnvironmentName()
        {
            return _appSettings.EnvironmentName;
        }

        /// <summary>
        /// Get IsDevelopment value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDevelopment()
        {
            return _appSettings.IsDevelopment;
        }

        /// <summary>
        /// Get IsProduction value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsProduction()
        {
            return _appSettings.IsProduction;
        }

        /// <summary>
        /// Get raw json string value
        /// </summary>
        /// <returns>string</returns>
        public string ToJson()
        {
            return _rawJson;
        }

        /// <summary>
        /// Get full json dynamic object
        /// </summary>
        /// <returns>T?</returns>
        /// <method>public T? ToObject&lt;T&gt;()</method>
        /// <exception>NullReferenceException</exception>
        public T ToObject<T>()
        {
            T? result = JsonConvert.DeserializeObject<T>(_rawJson);
            if (result == null)
                throw new NullReferenceException();

            return result;
        }
    }
}
