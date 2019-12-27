using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
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
    public class GetUserProfileRequest : IRequest<ApiResponse<UserDto>>
    {
    }

    public class GetUserProfileRequestHandler : BaseUserRequestHandler, IRequestHandler<GetUserProfileRequest, ApiResponse<UserDto>>
    {
        public GetUserProfileRequestHandler(IAsyncUserService userService,
                                            IIdentityService identityService) : base(userService, identityService)
        {

        }

        public async Task<ApiResponse<UserDto>> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = _identityService.GetUserIdentity();
            var getUserProfileRequestResponse = await _userService.GetProfileAsync(userId);

            if (getUserProfileRequestResponse.CompletedWithSuccess)
            {
                return getUserProfileRequestResponse;
            }

            else
            {
                return new ApiResponse<UserDto>()
                            .SetAsFailureResponse(getUserProfileRequestResponse.ErrorMessage);
            }
        }
    }
}
