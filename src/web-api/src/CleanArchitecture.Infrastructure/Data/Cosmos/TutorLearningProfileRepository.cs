using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Settings;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data.Cosmos
{
    public class TutorLearningProfileRepository : CosmosDbRepository<TutorLearningProfile>, IAsyncTutorLearningProfileRepository
    {
        public TutorLearningProfileRepository(ICosmosDbClientFactory cosmosDbClientFactory,
                                                           CosmosDbSettings cosmosDbSettings)
                                                                : base(cosmosDbClientFactory, cosmosDbSettings)
        {
        }

        public override string ContainerName => _cosmosDbSettings.TutorLearningProfilesContainerName;

        public async Task<TutorLearningProfile> GetByTutorIdAsync(Guid tutorId)
        {
            var sqlQueryText = $"SELECT p.id, p.pricePerHour, p.lessonTopicCategories FROM TutorLearningProfiles p WHERE p.tutorId = '{tutorId}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<TutorLearningProfile> queryResultSetIterator = _cosmosDbClientFactory
                                                                            .CosmosClient
                                                                            .GetContainer(_cosmosDbSettings.DatabaseName,
                                                                                          _cosmosDbSettings.TutorLearningProfilesContainerName)
                                                                            .GetItemQueryIterator<TutorLearningProfile>(queryDefinition);

            List<TutorLearningProfile> tutorLearningProfiles = new List<TutorLearningProfile>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TutorLearningProfile> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (TutorLearningProfile learningProfile in currentResultSet)
                {
                    tutorLearningProfiles.Add(learningProfile);
                }
            }

            var tutorLearningProfile = tutorLearningProfiles.FirstOrDefault();
            if (tutorLearningProfile != null)
            {
                tutorLearningProfile.TutorId = tutorId;
            }

            return tutorLearningProfile;
        }

        public async Task<IReadOnlyList<TutorLearningProfile>> ListAllByLessonTopicCategoryAsync(Guid lessonTopicCategoryId)
        {
            var sqlQueryText = $"SELECT p.id, p.pricePerHour, p.tutorId FROM TutorLearningProfiles p JOIN c IN p.lessonTopicCategories WHERE c.id = '{lessonTopicCategoryId}'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<TutorLearningProfile> queryResultSetIterator = _cosmosDbClientFactory
                                                                            .CosmosClient
                                                                            .GetContainer(_cosmosDbSettings.DatabaseName,
                                                                                          _cosmosDbSettings.TutorLearningProfilesContainerName)
                                                                            .GetItemQueryIterator<TutorLearningProfile>(queryDefinition);

            List<TutorLearningProfile> tutorLearningProfiles = new List<TutorLearningProfile>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TutorLearningProfile> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (TutorLearningProfile learningProfile in currentResultSet)
                {
                    tutorLearningProfiles.Add(learningProfile);
                }
            }

            return tutorLearningProfiles;
        }
    }
}
