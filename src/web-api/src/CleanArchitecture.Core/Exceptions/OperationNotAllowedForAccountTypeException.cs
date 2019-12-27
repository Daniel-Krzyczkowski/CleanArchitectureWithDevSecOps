using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Exceptions
{
    public class OperationNotAllowedForAccountTypeException : Exception
    {
        private readonly string _accountType;

        public string AccountType => _accountType;

        public OperationNotAllowedForAccountTypeException()
        {
        }

        public OperationNotAllowedForAccountTypeException(string accountType)
        {
            _accountType = accountType;
        }
    }
}
