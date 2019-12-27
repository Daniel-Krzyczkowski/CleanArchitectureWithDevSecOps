using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Providers
{
    public interface IMicrosoftGraphAuthenticationProvider
    {
        Task AuthenticateRequestAsync(HttpRequestMessage request);
    }
}
