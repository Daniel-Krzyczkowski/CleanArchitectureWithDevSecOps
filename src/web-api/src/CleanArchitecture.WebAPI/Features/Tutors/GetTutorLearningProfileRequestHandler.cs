using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
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
    public class GetTutorLearningProfileRequest : IRequest<ApiResponse<TutorLearningProfileDto>>
    {

    }

    public class GetTutorLearningProfileRequestHandler : BaseTutorRequestHandler,
                                                  IRequestHandler<GetTutorLearningProfileRequest, ApiResponse<TutorLearningProfileDto>>
    {
        public GetTutorLearningProfileRequestHandler(IAsyncUserService userService,
                                                      IAsyncTutorService tutorService,
                                                      IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {

        }

        public async Task<ApiResponse<TutorLearningProfileDto>> Handle(GetTutorLearningProfileRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var tutorId = _identityService.GetUserIdentity();

            var getTutorLearningProfilesRequestResponse = await _tutorService.GetLearningProfileAsync(tutorId);

            if (getTutorLearningProfilesRequestResponse.CompletedWithSuccess)
            {
                return new ApiResponse<TutorLearningProfileDto>
                {
                    Result = getTutorLearningProfilesRequestResponse.Result
                };
            }

            else
            {
                return new ApiResponse<TutorLearningProfileDto>()
                                    .SetAsFailureResponse(getTutorLearningProfilesRequestResponse.ErrorMessage);
            }
        }
    }
}
