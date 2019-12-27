using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncUserService
    {
        Task<ApiResponse<UserDto>> GetProfileAsync(Guid userId);
        Task<ApiResponse> UpdateProfileAsync(Guid userId, UpdateUserDto userProfileToUpdate);
        Task<ApiResponse> DeleteProfileAsync(Guid userId);
    }
}
