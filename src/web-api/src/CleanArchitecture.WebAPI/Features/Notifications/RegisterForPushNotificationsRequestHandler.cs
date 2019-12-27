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
    public class RegisterForPushNotificationsRequest : IRequest<ApiResponse>
    {
        public string RegistrationId { get; set; }
        public DeviceRegistration DeviceUpdate { get; set; }
    }

    public class RegisterForPushNotificationsRequestHandler : BaseNotificationRequestHandler,
                                                                IRequestHandler<RegisterForPushNotificationsRequest, ApiResponse>
    {
        public RegisterForPushNotificationsRequestHandler(IPushNotificationService pushNotificationService) : base(pushNotificationService)
        {
        }

        public async Task<ApiResponse> Handle(RegisterForPushNotificationsRequest request, CancellationToken cancellationToken)
        {
            var registrationId = request.RegistrationId;
            var deviceUpdate = request.DeviceUpdate;

            var registerForPushNotificationsRequestResult = await _pushNotificationService.RegisterForPushNotifications(registrationId,
                                                                                                                            deviceUpdate);
            return registerForPushNotificationsRequestResult;
        }
    }
}
