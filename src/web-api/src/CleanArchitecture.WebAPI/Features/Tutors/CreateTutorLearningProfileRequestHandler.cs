using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Tutors
{
    public class CreateTutorLearningProfileRequest : IRequest<ApiResponse<TutorLearningProfileDto>>
    {
        public AddTutorLearningProfileDto CreateTutorLearningProfileDto { get; set; }
    }

    public class CreateTutorLearningProfileRequestHandler : BaseTutorRequestHandler,
                                               IRequestHandler<CreateTutorLearningProfileRequest, ApiResponse<TutorLearningProfileDto>>
    {
        public CreateTutorLearningProfileRequestHandler(IAsyncUserService userService,
                                                        IAsyncTutorService tutorService,
                                                        IIdentityService identityService)
                                                        : base(userService, tutorService, identityService)
        {
        }

        public async Task<ApiResponse<TutorLearningProfileDto>> Handle(CreateTutorLearningProfileRequest request, CancellationToken cancellationToken)
        {
            VerifyAccountType();

            var createTutorLearningProfileDto = request.CreateTutorLearningProfileDto;

            var tutorId = _identityService.GetUserIdentity();

            var createTutorProfileRequestResponse = await _tutorService.AddLearningProfileAsync(tutorId, createTutorLearningProfileDto);
            return createTutorProfileRequestResponse;
        }
    }
}
