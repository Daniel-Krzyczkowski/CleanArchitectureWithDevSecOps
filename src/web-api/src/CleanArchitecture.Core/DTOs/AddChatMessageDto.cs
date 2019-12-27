using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.DTOs
{
    public class AddChatMessageDto
    {
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid ReceiverId { get; set; }
        [Required]
        public string MessageSubject { get; set; }
        [Required]
        public string MessageContent { get; set; }
    }
}
