using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.Constants;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Tutors
{
    public abstract class BaseTutorRequestHandler
    {
        protected readonly IAsyncUserService _userService;
        protected readonly IAsyncTutorService _tutorService;
        protected readonly IIdentityService _identityService;

        protected BaseTutorRequestHandler(IAsyncUserService userService,
                                          IAsyncTutorService tutorService,
                                          IIdentityService identityService)
        {
            _userService = userService;
            _tutorService = tutorService;
            _identityService = identityService;
        }

        protected void VerifyAccountType()
        {
            var accountType = _identityService.GetUserAccountType();
            if (!accountType.Equals(Constants.TutorAccountType))
            {
                throw new OperationNotAllowedForAccountTypeException(accountType);
            }
        }
    }
}
