using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Users
{
    public abstract class BaseUserRequestHandler
    {
        protected readonly IAsyncUserService _userService;
        protected readonly IIdentityService _identityService;

        protected BaseUserRequestHandler(IAsyncUserService userService,
                                         IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }
    }
}
