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

namespace CleanArchitecture.WebAPI.Features.Students
{
    public class GetLessonsHistoryForStudentRequest : IRequest<ApiResponse<IReadOnlyList<LessonDto>>>
    {
        public Guid StudentId { get; set; }
    }

    public class GetLessonsHistoryForStudentRequestHandler : BaseStudentRequestHandler,
                                                              IRequestHandler<GetLessonsHistoryForStudentRequest, ApiResponse<IReadOnlyList<LessonDto>>>
    {
        public GetLessonsHistoryForStudentRequestHandler(IAsyncUserService userService,
                                                         IAsyncStudentService studentService,
                                                         IIdentityService identityService)
                                                        : base(userService, studentService, identityService)
        {
        }

        public async Task<ApiResponse<IReadOnlyList<LessonDto>>> Handle(GetLessonsHistoryForStudentRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            request.StudentId = _identityService.GetUserIdentity();

            var studentLessonsHistoryRequestResult = await _studentService.GetLessonsHistoryAsync(request.StudentId);

            if (studentLessonsHistoryRequestResult.CompletedWithSuccess)
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>
                {
                    Result = studentLessonsHistoryRequestResult.Result
                };
            }

            else
            {
                return new ApiResponse<IReadOnlyList<LessonDto>>()
                                .SetAsFailureResponse(studentLessonsHistoryRequestResult.ErrorMessage);
            }
        }
    }
}
