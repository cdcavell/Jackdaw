namespace Jackdaw.IdentityServer.Models.AppSettings
{
    /// <summary>
    /// Google Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/05/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class Google
    {
        /// <value>string</value>
        public string ClientId { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
