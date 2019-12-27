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
    public class AcceptNewLessonByTutorRequest : IRequest<ApiResponse>
    {
        public Guid LessonId { get; set; }
    }

    public class AcceptNewLessonByTutorRequestHandler : BaseTutorRequestHandler,
                                                         IRequestHandler<AcceptNewLessonByTutorRequest, ApiResponse>
    {

        public AcceptNewLessonByTutorRequestHandler(IAsyncUserService userService,
                                                    IAsyncTutorService tutorService,
                                                    IIdentityService identityService) 
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse> Handle(AcceptNewLessonByTutorRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var tutorId = _identityService.GetUserIdentity();

            var acceptLessonRequestResult = await _tutorService.AcceptLessonAsync(tutorId, request.LessonId);
            return acceptLessonRequestResult;
        }
    }
}