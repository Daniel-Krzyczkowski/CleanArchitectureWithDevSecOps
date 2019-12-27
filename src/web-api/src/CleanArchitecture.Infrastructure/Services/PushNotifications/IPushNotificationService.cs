using CleanArchitecture.Core.Common;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.PushNotifications
{
    public interface IPushNotificationService
    {
        Task<ApiResponse<string>> CreateRegistrationId(string handle);
        Task<ApiResponse> DeleteRegistration(string registrationId);
        Task<ApiResponse> RegisterForPushNotifications(string registrationId, DeviceRegistration deviceUpdate);
        Task<ApiResponse<NotificationOutcome>> SendNotification(PushNotification newNotification);
    }
}
