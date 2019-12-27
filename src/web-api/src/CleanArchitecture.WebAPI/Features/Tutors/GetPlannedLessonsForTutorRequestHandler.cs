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
    public class GetPlannedLessonsForTutorRequest : IRequest<ApiResponse<IReadOnlyList<LessonDto>>>
    {
        public Guid TutorId { get; set; }
    }

    public class GetPlannedLessonsForTutorRequestHandler : BaseTutorRequestHandler,
                                                          IRequestHandler<GetPlannedLessonsForTutorRequest, ApiResponse<IReadOnlyList<LessonDto>>>

    {

        public GetPlannedLessonsForTutorRequestHandler(IAsyncUserService userService,
                                                       IAsyncTutorService tutorService,
                                                       IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> Handle(GetPlannedLessonsForTutorRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            request.TutorId = _identityService.GetUserIdentity();

            var plannedLessonsRequestResponse = await _tutorService.GetPlannedLessons(request.TutorId);

            if (plannedLessonsRequestResponse.CompletedWithSuccess)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>
                {
                    Result = plannedLessonsRequestResponse.Result
                };
            }

            else
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                            .SetAsFailureResponse(plannedLessonsRequestResponse.ErrorMessage);
            }
        }
    }
}
