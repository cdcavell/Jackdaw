namespace System
{
    /// <summary>
    /// Extension methods for existing Integer types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/11/2021 | Initial Development |~ 
    /// </revision>
    public static class IntegerExtensions

    {

        /// <summary>
        /// Method to return number of didgits in integer
        /// </summary>
        /// <param name="n">this int</param>
        /// <returns>int</returns>
        /// <method>Length(this int n)</method>
        public static int Length(this int n)
        {
            if (n < 0)
            {
                n = (n == Int32.MinValue) ? Int32.MaxValue : -n;
            }

            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;

            return 10;
        }

        /// <summary>
        /// Method to return int[] of integer
        /// </summary>
        /// <param name="n">this int</param>
        /// <returns>int[]</returns>
        /// <method>ToArray(this int n)</method>
        public static int[] ToArray(this int n)
        {
            var result = new int[n.Length()];
            for (int i = result.Length - 1; i >= 0; i--)
            {
                result[i] = n % 10;
                n /= 10;
            }

            return result;
        }
    }
}