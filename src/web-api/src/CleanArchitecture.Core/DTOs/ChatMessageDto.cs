using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.DTOs
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageSubject { get; set; }
        public string MessageContent { get; set; }
    }
}
