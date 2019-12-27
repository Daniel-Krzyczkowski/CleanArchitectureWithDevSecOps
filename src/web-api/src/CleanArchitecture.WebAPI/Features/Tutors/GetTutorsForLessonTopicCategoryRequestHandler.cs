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
    public class GetTutorsForLessonTopicCategoryRequest : IRequest<ApiResponse<IReadOnlyList<TutorLearningProfileDto>>>
    {
        public Guid lessonTopicCategoryId { get; set; }
    }

    public class GetTutorsForLessonTopicCategoryRequestHandler : BaseTutorRequestHandler,
                                                  IRequestHandler<GetTutorsForLessonTopicCategoryRequest, ApiResponse<IReadOnlyList<TutorLearningProfileDto>>>
    {
        public GetTutorsForLessonTopicCategoryRequestHandler(IAsyncUserService userService,
                                                             IAsyncTutorService tutorService,
                                                             IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse<IReadOnlyList<TutorLearningProfileDto>>> Handle(GetTutorsForLessonTopicCategoryRequest request, CancellationToken cancellationToken)
        {
            var getTutorsForLessonCategoryRequestResponse = await _tutorService.GetTutorsForLessonTopicCategoryAsync(request.lessonTopicCategoryId);

            if (getTutorsForLessonCategoryRequestResponse.CompletedWithSuccess)
            {
                return new ApiResponse<IReadOnlyList<TutorLearningProfileDto>>
                {
                    Result = getTutorsForLessonCategoryRequestResponse.Result
                };
            }

            else
            {
                return new ApiResponse<IReadOnlyList<TutorLearningProfileDto>>()
                                .SetAsFailureResponse(getTutorsForLessonCategoryRequestResponse.ErrorMessage);
            }
        }
    }
}
