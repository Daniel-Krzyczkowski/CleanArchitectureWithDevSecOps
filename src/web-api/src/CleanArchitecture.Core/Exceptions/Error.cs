using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Exceptions
{
    public sealed class Error
    {
        public string Title { get; }
        public string Message { get; }


        public Error(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
