namespace Jackdaw.Public.Models.AppSettings
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
    public class AppSettings : ClassLibrary.Mvc.Services.AppSettings.Models.AppSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <method>AppSettings(IConfiguration configuration)</method>
        public AppSettings(IConfiguration configuration) : base(configuration) { }
    }
}
