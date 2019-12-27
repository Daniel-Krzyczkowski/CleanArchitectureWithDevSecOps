using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
