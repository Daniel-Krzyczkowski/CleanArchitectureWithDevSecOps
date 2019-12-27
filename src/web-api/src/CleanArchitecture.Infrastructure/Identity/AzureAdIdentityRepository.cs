using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity.Model;
using CleanArchitecture.Infrastructure.Identity.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class AzureAdIdentityRepository : IAsyncIdentityRepository<User>
    {
        protected readonly IAdB2cGraphClientUserPropertiesProvider _adB2cGraphClientUserPropertiesProvider;

        public AzureAdIdentityRepository(IAdB2cGraphClientUserPropertiesProvider adB2cGraphClientUserPropertiesProvider)
        {
            _adB2cGraphClientUserPropertiesProvider = adB2cGraphClientUserPropertiesProvider;
        }
        public async Task<User> GetProfile(Guid id)
        {
            var userProfileResponse = await _adB2cGraphClientUserPropertiesProvider.GetUserByObjectId(id);

            if (userProfileResponse.CompletedWithSuccess)
            {
                var userProfile = userProfileResponse.Result;

                var student = new User
                {
                    Email = userProfile.signInNames.FirstOrDefault()?.value,
                    FirstName = userProfile.GivenName,
                    LastName = userProfile.Surname,
                    Id = id,
                    Phone = userProfile.Mobile,
                    AccountType = userProfile.AccountType
                };

                return student;
            }

            return null;
        }

        public async Task UpdateProfile(User student)
        {
            var userProfileToUpdate = new AdB2cUser
            {
                GivenName = student.FirstName,
                Surname = student.LastName,
                Mobile = student.Phone,
                signInNames = new List<SignInName>()
                {
                    new SignInName
                    {
                        type = "emailAddress",
                        value = student.Email
                    }
                }
            };

            await _adB2cGraphClientUserPropertiesProvider.UpdateUser(student.Id, userProfileToUpdate);
        }

        public Task DeleteProfile(Guid id)
        {
            return _adB2cGraphClientUserPropertiesProvider.DeleteUserByObjectId(id);
        }

        public async Task<IReadOnlyList<User>> GetProfiles(IList<Guid> identifiers)
        {
            var userProfilesResponse = await _adB2cGraphClientUserPropertiesProvider.GetUsersByObjectIdentifiers(identifiers);

            if (userProfilesResponse.CompletedWithSuccess)
            {
                var adB2cUserProfiles = userProfilesResponse.Result;

                return adB2cUserProfiles.Select(user => new User
                {
                    Email = user.signInNames?.FirstOrDefault()?.value,
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    Id = user.Id,
                    Phone = user.Mobile
                }).ToList();
            }

            return null;
        }
    }
}
