using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class ChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid UserId { get; set; }
        public string MessageSubject { get; set; }
        public string MessageContent { get; set; }
    }
}
