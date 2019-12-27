using CleanArchitecture.Core.Common;
using CleanArchitecture.Infrastructure.Services.PushNotifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Notifications
{
    public class CreateRegistrationIdRequest : IRequest<ApiResponse<string>>
    {
        public string Handle { get; set; }
    }

    public class CreateRegistrationIdRequestHandler : BaseNotificationRequestHandler,
                                                            IRequestHandler<CreateRegistrationIdRequest, ApiResponse<string>>
    {
        public CreateRegistrationIdRequestHandler(IPushNotificationService pushNotificationService) : base(pushNotificationService)
        {
        }

        public async Task<ApiResponse<string>> Handle(CreateRegistrationIdRequest request, CancellationToken cancellationToken)
        {
            var handle = request.Handle;

            var createRegistrationIdRequestResult = await _pushNotificationService.CreateRegistrationId(handle);
            return createRegistrationIdRequestResult;
        }
    }
}
