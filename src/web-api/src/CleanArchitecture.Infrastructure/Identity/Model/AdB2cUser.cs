using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Identity.Model
{
    public class AdB2cUser
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public IReadOnlyList<SignInName> signInNames { get; set; }
        public string Mobile { get; set; }
        public string AccountType { get; set; }
    }

    public class SignInName
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
