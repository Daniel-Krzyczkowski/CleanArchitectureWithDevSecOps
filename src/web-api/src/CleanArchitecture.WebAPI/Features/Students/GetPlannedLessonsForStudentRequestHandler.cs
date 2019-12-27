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

namespace CleanArchitecture.WebAPI.Features.Students
{
    public class GetPlannedLessonsForStudentRequest : IRequest<ApiResponse<IReadOnlyList<LessonDto>>>
    {
        public Guid StudentId { get; set; }
    }

    public class GetPlannedLessonsForStudentRequestHandler : BaseStudentRequestHandler,
                                                             IRequestHandler<GetPlannedLessonsForStudentRequest, ApiResponse<IReadOnlyList<LessonDto>>>

    {
        public GetPlannedLessonsForStudentRequestHandler(IAsyncUserService userService,
                                                         IAsyncStudentService studentService,
                                                         IIdentityService identityService)
                                                        : base(userService, studentService, identityService)
        {
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> Handle(GetPlannedLessonsForStudentRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            request.StudentId = _identityService.GetUserIdentity();

            var plannedLessonsRequestResponse = await _studentService.GetPlannedLessons(request.StudentId);

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
