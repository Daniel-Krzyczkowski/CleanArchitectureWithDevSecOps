using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Settings
{
    public class NotificationHubSettings
    {
        public string HubName { get; set; }
        public string HubDefaultFullSharedAccessSignature { get; set; }
    }
}
