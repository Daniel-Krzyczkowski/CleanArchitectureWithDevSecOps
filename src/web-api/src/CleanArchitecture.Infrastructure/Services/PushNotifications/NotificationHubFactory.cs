using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Infrastructure.Settings;
using Microsoft.Azure.NotificationHubs;

namespace CleanArchitecture.Infrastructure.Services.PushNotifications
{
    public class NotificationHubFactory : INotificationHubFactory
    {
        private readonly NotificationHubSettings _notificationHubSettings;

        public NotificationHubClient NotificationHubClient { get; }

        public NotificationHubFactory(NotificationHubSettings notificationHubSettings)
        {
            _notificationHubSettings = notificationHubSettings;
            NotificationHubClient = NotificationHubClient.CreateClientFromConnectionString(_notificationHubSettings.HubDefaultFullSharedAccessSignature,
                                                                       _notificationHubSettings.HubName);
        }
    }
}
