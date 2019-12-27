using CleanArchitecture.Core.Common;
using CleanArchitecture.Infrastructure.Identity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Providers
{
    public interface IAdB2cGraphClientUserPropertiesProvider
    {
        Task<ApiResponse<AdB2cUser>> GetUserByObjectId(Guid objectId);
        Task<ApiResponse> UpdateUser(Guid objectId, AdB2cUser adB2cUser);
        Task<ApiResponse> DeleteUserByObjectId(Guid objectId);
        Task<ApiResponse<IReadOnlyList<AdB2cUser>>> GetUsersByObjectIdentifiers(IList<Guid> identifiers);
    }
}
