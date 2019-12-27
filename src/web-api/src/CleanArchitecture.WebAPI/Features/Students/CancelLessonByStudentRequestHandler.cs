using CleanArchitecture.Core.Common;
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
    public class CancelLessonByStudentRequest : IRequest<ApiResponse>
    {
        public Guid LessonId { get; set; }
    }

    public class CancelLessonByStudentRequestHandler : BaseStudentRequestHandler,
                                                         IRequestHandler<CancelLessonByStudentRequest, ApiResponse>
    {
        public CancelLessonByStudentRequestHandler(IAsyncUserService userService,
                                                   IAsyncStudentService studentService,
                                                   IIdentityService identityService) 
                                                        : base(userService, studentService, identityService)
        {
        }

        public async Task<ApiResponse> Handle(CancelLessonByStudentRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var studentId = _identityService.GetUserIdentity();

            var cancelLessonRequestResult = await _studentService.CancelLessonAsync(studentId, request.LessonId);
            return cancelLessonRequestResult;
        }
    }
}
