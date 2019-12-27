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
    public class DeleteTutorLearningProfileRequest : IRequest<ApiResponse>
    {
    }

    public class DeleteTutorLearningProfileRequestHandler : BaseTutorRequestHandler,
                                                     IRequestHandler<DeleteTutorLearningProfileRequest, ApiResponse>
    {
        public DeleteTutorLearningProfileRequestHandler(IAsyncUserService userService,
                                                        IAsyncTutorService tutorService,
                                                        IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse> Handle(DeleteTutorLearningProfileRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var tutorId = _identityService.GetUserIdentity();

            var deleteTutortProfileRequestResponse = await _tutorService.DeleteLearningProfileAsync(tutorId);
            return deleteTutortProfileRequestResponse;
        }
    }
}
