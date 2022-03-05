﻿using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Text;

namespace Jackdaw.IdentityServer.Models.Diagnostics
{
    /// <summary>
    /// Diagnostics View Model
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/04/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class DiagnosticsViewModel
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="result">AuthenticateResult</param>
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            AuthenticateResult = result;

            if (result.Properties != null)
                if (result.Properties.Items.ContainsKey("client_list"))
                {
                    var encoded = result.Properties.Items["client_list"];
                    var bytes = Base64Url.Decode(encoded);
                    var value = Encoding.UTF8.GetString(bytes);

                    if (value != null)
                    {
                        var clients = JsonConvert.DeserializeObject<string[]>(value);
                        if (clients != null)
                            Clients = clients.ToList();
                    }
                }
        }

        /// <value>AuthenticateResult</value>
        public AuthenticateResult AuthenticateResult { get; }
        /// <value>Clients</value>
        public IEnumerable<string> Clients { get; } = new List<string>();
    }
}
