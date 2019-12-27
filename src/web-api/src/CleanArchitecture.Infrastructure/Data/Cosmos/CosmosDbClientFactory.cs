using Microsoft.Azure.Cosmos;

namespace CleanArchitecture.Infrastructure.Data.Cosmos
{
    public class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private readonly CosmosClient _cosmosClient;

        public CosmosDbClientFactory(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public CosmosClient CosmosClient => _cosmosClient;
    }
}
