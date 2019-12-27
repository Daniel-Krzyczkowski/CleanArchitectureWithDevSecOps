using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data.Cosmos
{
    public interface ICosmosDbClientFactory
    {
        public CosmosClient CosmosClient { get; }
    }
}
