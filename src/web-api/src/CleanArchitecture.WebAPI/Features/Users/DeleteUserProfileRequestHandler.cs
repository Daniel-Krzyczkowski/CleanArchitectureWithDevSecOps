using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Users
{
    public class DeleteUserProfileRequest : IRequest<ApiResponse>
    {
    }

    public class DeleteUserProfileRequestHandler : BaseUserRequestHandler, IRequestHandler<DeleteUserProfileRequest, ApiResponse>
    {
        public DeleteUserProfileRequestHandler(IAsyncUserService userService,
                                               IIdentityService identityService) : base(userService, identityService)
        {

        }

        public async Task<ApiResponse> Handle(DeleteUserProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = _identityService.GetUserIdentity();

            var deleteUserProfileRequestResponse = await _userService.DeleteProfileAsync(userId);
            return deleteUserProfileRequestResponse;
        }
    }
}
