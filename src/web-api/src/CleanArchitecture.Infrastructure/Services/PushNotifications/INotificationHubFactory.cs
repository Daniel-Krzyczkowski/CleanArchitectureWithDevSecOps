using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.PushNotifications
{
    public interface INotificationHubFactory
    {
        public NotificationHubClient NotificationHubClient { get; }
    }
}
