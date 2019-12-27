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
    public class GetLessonsHistoryForTutorRequest : IRequest<ApiResponse<IReadOnlyList<LessonDto>>>
    {
        public Guid TutorId { get; set; }
    }

    public class GetLessonsHistoryForTutorRequestHandler : BaseTutorRequestHandler,
                                                           IRequestHandler<GetLessonsHistoryForTutorRequest, ApiResponse<IReadOnlyList<LessonDto>>>
    {

        public GetLessonsHistoryForTutorRequestHandler(IAsyncUserService userService,
                                                       IAsyncTutorService tutorService,
                                                       IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> Handle(GetLessonsHistoryForTutorRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            VerifyAccountType();

            request.TutorId = _identityService.GetUserIdentity();

            var tutorLessonsHistoryRequestResult = await _tutorService.GetLessonsHistoryAsync(request.TutorId);

            if (tutorLessonsHistoryRequestResult.CompletedWithSuccess)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>
                {
                    Result = tutorLessonsHistoryRequestResult.Result
                };
            }

            else
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                                    .SetAsFailureResponse(tutorLessonsHistoryRequestResult.ErrorMessage);
            }
        }
    }
}
