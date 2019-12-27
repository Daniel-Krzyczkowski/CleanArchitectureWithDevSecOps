using CleanArchitecture.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Common
{
    public class ApiResponse
    {
        protected bool _forcedFailedResponse;
        public bool CompletedWithSuccess => ErrorMessage == null && !_forcedFailedResponse;
        public Error ErrorMessage { get; set; }

        public ApiResponse SetAsFailureResponse(Error errorMessage)
        {
            ErrorMessage = errorMessage;
            _forcedFailedResponse = true;
            return this;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Result { get; set; }

        public new ApiResponse<T> SetAsFailureResponse(Error errorMessage)
        {
            base.SetAsFailureResponse(errorMessage);
            return this;
        }
    }
}
