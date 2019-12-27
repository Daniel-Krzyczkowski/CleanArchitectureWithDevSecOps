using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Features.Chat
{
    public class SaveChatMessageRequest : IRequest<ApiResponse<ChatMessageDto>>
    {
        public Guid UserId { get; set; }
        public AddChatMessageDto ChatMessage { get; set; }
    }

    public class SaveChatMessageRequestHandler : IRequestHandler<SaveChatMessageRequest, ApiResponse<ChatMessageDto>>
    {
        private readonly IAsyncChatService _chatService;

        public SaveChatMessageRequestHandler(IAsyncChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<ApiResponse<ChatMessageDto>> Handle(SaveChatMessageRequest request, CancellationToken cancellationToken)
        {
            var newChatMessageDto = request.ChatMessage;
            var userId = request.UserId;

            var addChatMessageRequestResponse = await _chatService.SaveMessageAsync(userId, newChatMessageDto);
            return addChatMessageRequestResponse;
        }
    }
}
