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
    public class UnregisterFromPushNotificationsRequest : IRequest<ApiResponse>
    {
        public string RegistrationId { get; set; }
    }

    public class UnregisterFromPushNotificationsRequestHandler : BaseNotificationRequestHandler,
                                                                    IRequestHandler<UnregisterFromPushNotificationsRequest, ApiResponse>
    {
        public UnregisterFromPushNotificationsRequestHandler(IPushNotificationService pushNotificationService) : base(pushNotificationService)
        {
        }

        public async Task<ApiResponse> Handle(UnregisterFromPushNotificationsRequest request, CancellationToken cancellationToken)
        {
            var registrationId = request.RegistrationId;

            var unregisterFromPushNotificationsRequestResult = await _pushNotificationService.DeleteRegistration(registrationId);
            return unregisterFromPushNotificationsRequestResult;
        }
    }
}
