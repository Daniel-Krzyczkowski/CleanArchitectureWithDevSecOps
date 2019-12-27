using CleanArchitecture.Core.Common;
using CleanArchitecture.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IAsyncChatService
    {
        Task<ApiResponse<ChatMessageDto>> SaveMessageAsync(Guid userId, AddChatMessageDto chatMessage);
    }
}
