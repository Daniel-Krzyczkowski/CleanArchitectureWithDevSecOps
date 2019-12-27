using CleanArchitecture.Infrastructure.Settings;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Providers
{
    public class MicrosoftGraphApiAuthenticationProvider : IMicrosoftGraphAuthenticationProvider
    {
        private readonly MicrosoftGraphSettings _settings;

        public MicrosoftGraphApiAuthenticationProvider(MicrosoftGraphSettings settings)
        {
            _settings = settings;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var authContext = new AuthenticationContext($"https://login.microsoftonline.com/{_settings.AzureAdB2CTenant}");

            var clientCred = new ClientCredential(_settings.ClientId, _settings.ClientSecret);

            var authResult = await authContext.AcquireTokenAsync(_settings.ApiUrl, clientCred);

            if (authResult == null)
            {
                Log.Error("Failed to obtain the JWT token from the Microsoft Graph");
                throw new InvalidOperationException("Failed to obtain the JWT token from the Microsoft Graph");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }
    }
}
