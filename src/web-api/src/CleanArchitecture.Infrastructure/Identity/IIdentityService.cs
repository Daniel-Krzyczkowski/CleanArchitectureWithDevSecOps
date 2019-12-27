using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Identity
{
    public interface IIdentityService
    {
        Guid GetUserIdentity();
        string GetUserEmail();
        string GetUserAccountType();
    }
}
