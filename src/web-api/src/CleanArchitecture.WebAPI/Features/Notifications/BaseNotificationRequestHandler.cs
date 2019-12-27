using CleanArchitecture.Infrastructure.Services.PushNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Notifications
{
    public class BaseNotificationRequestHandler
    {
        protected readonly IPushNotificationService _pushNotificationService;

        public BaseNotificationRequestHandler(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }
    }
}
