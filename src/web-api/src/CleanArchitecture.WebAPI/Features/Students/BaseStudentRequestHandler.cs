using CleanArchitecture.Core.Constants;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Students
{
    public abstract class BaseStudentRequestHandler
    {
        protected readonly IAsyncUserService _userService;
        protected readonly IAsyncStudentService _studentService;
        protected readonly IIdentityService _identityService;

        protected BaseStudentRequestHandler(IAsyncUserService userService,
                                            IAsyncStudentService studentService,
                                            IIdentityService identityService)
        {
            _userService = userService;
            _studentService = studentService;
            _identityService = identityService;
        }

        protected void VerifyAccountType()
        {
            var accountType = _identityService.GetUserAccountType();
            if (!accountType.Equals(Constants.StudentAccountType))
            {
                throw new OperationNotAllowedForAccountTypeException(accountType);
            }
        }
    }
}
