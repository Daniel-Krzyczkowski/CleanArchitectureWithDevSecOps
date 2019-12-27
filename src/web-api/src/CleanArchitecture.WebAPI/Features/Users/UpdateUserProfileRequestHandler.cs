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
    public class UpdateUserProfileRequest : IRequest<ApiResponse>
    {
        public UpdateUserDto UpdateUserProfileDto { get; set; }
    }

    public class UpdateUserProfileRequestHandler : BaseUserRequestHandler, IRequestHandler<UpdateUserProfileRequest, ApiResponse>
    {
        public UpdateUserProfileRequestHandler(IAsyncUserService userService,
                                               IIdentityService identityService) : base(userService, identityService)
        {

        }

        public async Task<ApiResponse> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = _identityService.GetUserIdentity();

            var updateStudentProfileDto = request.UpdateUserProfileDto;

            var getStudentProfileRequestResponse = await _userService.UpdateProfileAsync(userId, updateStudentProfileDto);
            return getStudentProfileRequestResponse;
        }
    }
}
