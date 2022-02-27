namespace Jackdaw.IdentityServer.Models.AppSettings
{
    /// <summary>
    /// Application settings information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 02/27/2022 | Duende IdentityServer Integration |~ 
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
