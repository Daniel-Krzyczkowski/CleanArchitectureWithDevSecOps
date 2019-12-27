using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Identity.Model
{
    public class AdB2cUsersIdentifiers
    {
        [JsonProperty("ids")]
        public IList<Guid> Identifiers { get; set; }
        [JsonProperty("types")]
        public IList<string> Types { get; set; }
    }
}
