namespace System
{
    /// <summary>
    /// Extension methods for existing DateTime types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/11/2021 | Initial Development |~ 
    /// </revision>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Method to return timestamp of current DateTime value
        /// </summary>
        /// <param name="value">this DateTime</param>
        /// <returns>string</returns>
        /// <method>Timestamp(this DateTime value)</method>
        public static string Timestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
