using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Settings
{
    public class AzureAdB2CSettings
    {
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string Policy { get; set; }
    }
}
