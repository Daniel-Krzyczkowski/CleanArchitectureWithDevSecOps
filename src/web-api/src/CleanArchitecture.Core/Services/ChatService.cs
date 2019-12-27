using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Services
{
    public class ChatService : IAsyncChatService
    {
        private readonly IAsyncRepository<ChatMessage> _chatMessagesRepository;
        public ChatService(IAsyncRepository<ChatMessage> chatMessagesRepository)
        {
            _chatMessagesRepository = chatMessagesRepository;
        }

        public async Task<ApiResponse<ChatMessageDto>> SaveMessageAsync(Guid userId, AddChatMessageDto chatMessage)
        {
            var newMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MessageContent = chatMessage.MessageContent,
                MessageSubject = chatMessage.MessageSubject,
                ReceiverId = chatMessage.ReceiverId,
                SenderId = chatMessage.SenderId
            };

            var savedMessage = await _chatMessagesRepository.AddAsync(newMessage);

            if (savedMessage != null)
            {
                var addedMessage = new ChatMessageDto
                {
                    Id = savedMessage.Id,
                    SenderId = savedMessage.SenderId,
                    ReceiverId = savedMessage.ReceiverId,
                    MessageSubject = savedMessage.MessageSubject,
                    MessageContent = savedMessage.MessageContent
                };

                return new ApiResponse<ChatMessageDto>
                {
                    Result = addedMessage
                };
            }

            else
            {
                return new ApiResponse<ChatMessageDto>()
                    .SetAsFailureResponse(Errors.Chat.ChatMessageCannotBeSaved());
            }
        }
    }
}
