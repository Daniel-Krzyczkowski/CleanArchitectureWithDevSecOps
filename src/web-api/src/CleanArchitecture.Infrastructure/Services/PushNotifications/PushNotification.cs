using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.PushNotifications
{
    public class PushNotification
    {
        public string Message { get; set; }
        public IList<string> Tags { get; set; }
        public MobilePlatform MobilePlatform { get; set; }
    }
}
