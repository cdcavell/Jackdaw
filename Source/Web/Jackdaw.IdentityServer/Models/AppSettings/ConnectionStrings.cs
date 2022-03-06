namespace Jackdaw.IdentityServer.Models.AppSettings
{
    /// <summary>
    /// ConnectionStrings model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class ConnectionStrings
    {
        /// <value>string</value>
        public string EntityFrameworkConnection { get; set; } = string.Empty;
    }
}
