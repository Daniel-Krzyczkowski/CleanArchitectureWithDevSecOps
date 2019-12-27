using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Cosmos
{
    public class ChatMessageRepository : CosmosDbRepository<ChatMessage>
    {
        public override string ContainerName => _cosmosDbSettings.ChatMessagesContainerName;

        public ChatMessageRepository(ICosmosDbClientFactory cosmosDbClientFactory,
                                                   CosmosDbSettings cosmosDbSettings)
                                                        : base(cosmosDbClientFactory, cosmosDbSettings)
        {
        }
    }
}
