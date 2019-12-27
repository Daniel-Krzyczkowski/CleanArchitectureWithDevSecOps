using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Identity.Model
{
    public class AdB2cUsersList
    {
        [JsonProperty("value")]
        public IReadOnlyList<AdB2cUser> Users { get; set; }
    }
}
