using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Settings
{
    public class MicrosoftGraphSettings
    {
        public string AzureAdB2CTenant { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiUrl { get; set; }
        public string ApiVersion { get; set; }
    }
}
