namespace Jackdaw.ClassLibrary.Mvc.Services.AppSettings
{
    /// <summary>
    /// AppSettings Web Service Interface
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/12/2021 | Initial Development |~ 
    /// </revision>
    public interface IAppSettingsService
    {
        /// <summary>
        /// Get AssemblyName value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyName();

        /// <summary>
        /// Get AssemblyVersion value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyVersion();

        /// <summary>
        /// Get LastModifiedDate value
        /// </summary>
        /// <returns>string</returns>
        public string LastModifiedDate();

        /// <summary>
        /// Get Author's age
        /// </summary>
        /// <returns>int</returns>
        public int AuthorAge();

        /// <summary>
        /// Get EnvironmentName value
        /// </summary>
        /// <returns>string</returns>
        public string EnvironmentName();

        /// <summary>
        /// Get IsDevelopment value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDevelopment();

        /// <summary>
        /// Get IsProduction value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsProduction();

        /// <summary>
        /// Get raw json string value
        /// </summary>
        /// <returns>string</returns>
        public string ToJson();

        /// <summary>
        /// Get dynamic object
        /// </summary>
        /// <returns>T?</returns>
        /// <method>public T ToObject&lt;T&gt;()</method>
        public T ToObject<T>();
    }
}
