using CleanArchitecture.Core.Common;
using CleanArchitecture.Infrastructure.Services.PushNotifications;
using MediatR;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Notifications
{
    public class SendPushNotificationRequest : IRequest<ApiResponse<NotificationOutcome>>
    {
        public PushNotification NewNotification { get; set; }
    }

    public class SendPushNotificationRequestHandler : BaseNotificationRequestHandler,
                                                            IRequestHandler<SendPushNotificationRequest, ApiResponse<NotificationOutcome>>
    {
        public SendPushNotificationRequestHandler(IPushNotificationService pushNotificationService) : base(pushNotificationService)
        {
        }

        public async Task<ApiResponse<NotificationOutcome>> Handle(SendPushNotificationRequest request, CancellationToken cancellationToken)
        {
            var notificationToSend = request.NewNotification;

            var sendPushNotificationRequestResult = await _pushNotificationService.SendNotification(notificationToSend);
            return sendPushNotificationRequestResult;
        }
    }
}
