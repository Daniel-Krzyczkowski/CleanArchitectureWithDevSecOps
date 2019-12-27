using CleanArchitecture.Core.Common;
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
    public class CancelLessonByTutorRequest : IRequest<ApiResponse>
    {
        public Guid LessonId { get; set; }
        public Guid TutorId { get; set; }
    }

    public class CancelLessonByTutorRequestHandler : BaseTutorRequestHandler,
                                                      IRequestHandler<CancelLessonByTutorRequest, ApiResponse>
    {
        public CancelLessonByTutorRequestHandler(IAsyncUserService userService,
                                                 IAsyncTutorService tutorService,
                                                 IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse> Handle(CancelLessonByTutorRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            request.TutorId = _identityService.GetUserIdentity();

            var cancelLessonRequestResult = await _tutorService.CancelLessonAsync(request.TutorId, request.LessonId);
            return cancelLessonRequestResult;
        }
    }
}
