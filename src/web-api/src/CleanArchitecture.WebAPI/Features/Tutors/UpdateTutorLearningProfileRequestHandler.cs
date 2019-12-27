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

namespace CleanArchitecture.WebAPI.Features.Tutors
{
    public class UpdateTutorLearningProfileRequest : IRequest<ApiResponse>
    {
        public UpdateTutorLearningProfileDto UpdateTutorLearningProfileDto { get; set; }
    }

    public class UpdateTutorLearningProfileRequestHandler : BaseTutorRequestHandler,
                                                        IRequestHandler<UpdateTutorLearningProfileRequest, ApiResponse>
    {
        public UpdateTutorLearningProfileRequestHandler(IAsyncUserService userService,
                                                        IAsyncTutorService tutorService,
                                                        IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse> Handle(UpdateTutorLearningProfileRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var updateTutorLearningProfileDto = request.UpdateTutorLearningProfileDto;

            var tutorId = _identityService.GetUserIdentity();

            var getTutorProfileRequestResponse = await _tutorService.UpdateLearningProfileAsync(tutorId, updateTutorLearningProfileDto);
            return getTutorProfileRequestResponse;
        }
    }
}
