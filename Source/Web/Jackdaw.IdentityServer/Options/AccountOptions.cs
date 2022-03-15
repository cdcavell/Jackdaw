namespace Jackdaw.IdentityServer.Options
{
    /// <summary>
    /// Account Options
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/15/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class AccountOptions
    {
        private static bool allowLocalLogin = true;
        private static bool allowRememberLogin = true;
        private static TimeSpan rememberMeLoginDuration = TimeSpan.FromDays(30);
        private static bool showLogoutPrompt = false;
        private static bool automaticRedirectAfterSignOut = false;
        private static string invalidCredentialsErrorMessage = "Invalid username or password";

        /// <value>bool</value>
        public static bool AllowLocalLogin { get => allowLocalLogin; set => allowLocalLogin = value; }
        /// <value>bool</value>
        public static bool AllowRememberLogin { get => allowRememberLogin; set => allowRememberLogin = value; }
        /// <value>TimeSpan</value>
        public static TimeSpan RememberMeLoginDuration { get => rememberMeLoginDuration; set => rememberMeLoginDuration = value; }

        /// <value>bool</value>
        public static bool ShowLogoutPrompt { get => showLogoutPrompt; set => showLogoutPrompt = value; }
        /// <value>bool</value>
        public static bool AutomaticRedirectAfterSignOut { get => automaticRedirectAfterSignOut; set => automaticRedirectAfterSignOut = value; }

        /// <value>string</value>
        public static string InvalidCredentialsErrorMessage { get => invalidCredentialsErrorMessage; set => invalidCredentialsErrorMessage = value; }

    }
}
