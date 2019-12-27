using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Services
{
    public class UserService : IAsyncUserService
    {
        private readonly IAsyncIdentityRepository<User> _userIdentityRepository;

        public UserService(IAsyncIdentityRepository<User> userIdentityRepository)
        {
            _userIdentityRepository = userIdentityRepository;
        }

        public async Task<ApiResponse<UserDto>> GetProfileAsync(Guid userId)
        {
            var userProfile = await _userIdentityRepository.GetProfile(userId);
            if (userProfile == null)
            {
                return new ApiResponse<UserDto>().SetAsFailureResponse(Errors.User.UserProfileNotFound());
            }

            else
            {
                var userProfileDto = new UserDto
                {
                    Id = userProfile.Id,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    AccountType = userProfile.AccountType,
                    Email = userProfile.Email,
                    Phone = userProfile.Phone
                };

                return new ApiResponse<UserDto>
                {
                    Result = userProfileDto
                };
            }
        }

        public async Task<ApiResponse> UpdateProfileAsync(Guid userId, UpdateUserDto userProfileToUpdate)
        {
            var userProfile = await _userIdentityRepository.GetProfile(userId);
            if (userProfile == null)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.User.UserProfileNotFound());
            }

            else
            {
                userProfile.FirstName = userProfileToUpdate.FirstName;
                userProfile.LastName = userProfileToUpdate.LastName;
                userProfile.Phone = userProfileToUpdate.Phone;

                await _userIdentityRepository.UpdateProfile(userProfile);

                return new ApiResponse();
            }
        }

        public async Task<ApiResponse> DeleteProfileAsync(Guid userId)
        {
            var userProfile = await _userIdentityRepository.GetProfile(userId);
            if (userProfile == null)
            {
                return new ApiResponse().SetAsFailureResponse(Errors.User.UserProfileNotFound());
            }

            else
            {
                await _userIdentityRepository.DeleteProfile(userId);

                return new ApiResponse();
            }
        }
    }
}
