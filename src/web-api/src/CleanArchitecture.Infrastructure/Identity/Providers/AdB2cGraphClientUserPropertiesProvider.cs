using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Infrastructure.Settings;
using CleanArchitecture.Infrastructure.Identity.Model;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Infrastructure.Identity.Providers
{
    public class AdB2cGraphClientUserPropertiesProvider : IAdB2cGraphClientUserPropertiesProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IAzureAdGraphAuthenticationProvider _azureAdGraphAuthenticationProvider;
        private readonly IMicrosoftGraphAuthenticationProvider _microsoftGraphAuthenticationProvider;
        private readonly AzureAdGraphSettings _azureAdGraphSettings;
        private readonly MicrosoftGraphSettings _microsoftGraphSettings;

        public AdB2cGraphClientUserPropertiesProvider(HttpClient httpClient,
                                                      IAzureAdGraphAuthenticationProvider azureAdGraphAuthenticationProvider,
                                                      IMicrosoftGraphAuthenticationProvider microsoftGraphAuthenticationProvider,
                                                      AzureAdGraphSettings azureAdGraphSettings,
                                                      MicrosoftGraphSettings microsoftGraphSettings)
        {
            _httpClient = httpClient;
            _azureAdGraphAuthenticationProvider = azureAdGraphAuthenticationProvider;
            _microsoftGraphAuthenticationProvider = microsoftGraphAuthenticationProvider;
            _azureAdGraphSettings = azureAdGraphSettings;
            _microsoftGraphSettings = microsoftGraphSettings;
        }

        public async Task<ApiResponse<AdB2cUser>> GetUserByObjectId(Guid objectId)
        {
            string url = $"{_azureAdGraphSettings.ApiUrl}/{_azureAdGraphSettings.AzureAdB2CTenant}/users/{objectId}?{_azureAdGraphSettings.ApiVersion}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            await _azureAdGraphAuthenticationProvider.AuthenticateRequestAsync(request);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                Log.Error($"An error ocurred when getting user profile with Azure AD Graph API: {error}");

                return new ApiResponse<AdB2cUser>()
                                    .SetAsFailureResponse(Errors.User.UserProfileCannotBeLoaded());
            }

            else
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var adB2cUserJsonData = JObject.Parse(stringResponse);

                var userAccountType = adB2cUserJsonData[$"extension_{_azureAdGraphSettings.ExtensionsAppClientId}_account_type"].ToString();
                var adB2cUser = JsonConvert.DeserializeObject<AdB2cUser>(stringResponse);

                adB2cUser.AccountType = userAccountType;

                return new ApiResponse<AdB2cUser>
                {
                    Result = adB2cUser
                };
            }
        }

        public async Task<ApiResponse> UpdateUser(Guid objectId, AdB2cUser adB2cUser)
        {
            string url = $"{_azureAdGraphSettings.ApiUrl}/{_azureAdGraphSettings.AzureAdB2CTenant}/users/{objectId}?{_azureAdGraphSettings.ApiVersion}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            await _azureAdGraphAuthenticationProvider.AuthenticateRequestAsync(request);

            var adB2cUserUpdate = new JObject
            {
                { "givenName", adB2cUser.GivenName },
                { "surname", adB2cUser.Surname },
                { "mobile", adB2cUser.Mobile }
            }.ToString();

            request.Content = new StringContent(adB2cUserUpdate, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                Log.Error($"An error ocurred when updating user profile with Azure AD Graph API: {error}");

                return new ApiResponse().SetAsFailureResponse(Errors.User.UserProfileCannotBeUpdated());
            }

            else
            {
                return new ApiResponse();
            }
        }

        public async Task<ApiResponse> DeleteUserByObjectId(Guid objectId)
        {
            string url = $"{_azureAdGraphSettings.ApiUrl}/{_azureAdGraphSettings.AzureAdB2CTenant}/users/{objectId}?{_azureAdGraphSettings.ApiVersion}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            await _azureAdGraphAuthenticationProvider.AuthenticateRequestAsync(request);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                Log.Error($"An error ocurred when deleting user profile with Azure AD Graph API: {error}");

                return new ApiResponse()
                            .SetAsFailureResponse(Errors.User.UserProfileCannotBeDeleted());
            }

            else
            {
                return new ApiResponse();
            }
        }

        public async Task<ApiResponse<IReadOnlyList<AdB2cUser>>> GetUsersByObjectIdentifiers(IList<Guid> identifiers)
        {
            string url = $"{_microsoftGraphSettings.ApiUrl}{_microsoftGraphSettings.ApiVersion}/directoryObjects/getByIds";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), url);
            await _microsoftGraphAuthenticationProvider.AuthenticateRequestAsync(request);

            var adB2cUsersIdentifiers = new AdB2cUsersIdentifiers
            {
                Types = new List<string> { "user" },
                Identifiers = identifiers
            };

            var adB2cUsersIdentifiersAsJson = JsonConvert.SerializeObject(adB2cUsersIdentifiers);

            request.Content = new StringContent(adB2cUsersIdentifiersAsJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                Log.Error($"An error ocurred when getting user profiles by identifiers with Azure AD Graph API: {error}");

                return new ApiResponse<IReadOnlyList<AdB2cUser>>()
                                .SetAsFailureResponse(Errors.User.UserProfileCannotBeLoaded());
            }

            else
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var adB2cUsersList = JsonConvert.DeserializeObject<AdB2cUsersList>(stringResponse);

                foreach (var adUser in adB2cUsersList.Users)
                {
                    if (adUser.signInNames == null || adUser.Mobile == null)
                    {
                        var completeUserProfile = await GetUserByObjectId(adUser.Id);
                        if (completeUserProfile.CompletedWithSuccess)
                        {
                            adUser.signInNames = completeUserProfile.Result.signInNames;
                            adUser.Mobile = completeUserProfile.Result.Mobile;
                        }
                    }
                }

                return new ApiResponse<IReadOnlyList<AdB2cUser>>
                {
                    Result = adB2cUsersList.Users
                };
            }
        }
    }
}
