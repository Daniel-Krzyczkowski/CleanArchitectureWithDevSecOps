using CleanArchitecture.Core.DTOs;
using CleanArchitecture.WebAPI.Features.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        protected readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HubMethodName("SendDirectMessageToUser")]
        public async Task SendDirectMessageToUser(string chatMessageAsJson)
        {
            var chatMessage = JsonConvert.DeserializeObject<AddChatMessageDto>(chatMessageAsJson);
            chatMessage.SenderId = new Guid(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _mediator.Send(new SaveChatMessageRequest
            {
                ChatMessage = chatMessage,
                UserId = chatMessage.SenderId
            });

            var messageAsJson = JsonConvert.SerializeObject(chatMessage);

            await Clients.User(chatMessage.ReceiverId.ToString()).SendAsync(messageAsJson);
        }
    }
}
