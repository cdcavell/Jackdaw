namespace Jackdaw.IdentityServer.Models.AppSettings
{
    /// <summary>
    /// Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/05/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class Authentication
    {
        /// <value>Googel</value>
        public Google Google { get; set; } = new();
    }
}
