using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data.Ef;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Services;
using CleanArchitecture.WebAPI.Features.Tutors;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Core.Exceptions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CleanArchitecture.Infrastructure.Identity.Providers;
using CleanArchitecture.Infrastructure.Settings;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;
using CleanArchitecture.Infrastructure.Data.Cosmos;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Services.PushNotifications;

namespace CleanArchitecture.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var azureAdGraphConfiguration = new AzureAdGraphSettings()
            {
                AzureAdB2CTenant = configuration["AzureAdGraph:AzureAdB2CTenant"],
                ClientId = configuration["AzureAdGraph:ClientId"],
                ClientSecret = configuration["AzureAdGraph:ClientSecret"],
                PolicyName = configuration["AzureAdGraph:PolicyName"],
                ApiUrl = configuration["AzureAdGraph:ApiUrl"],
                ApiVersion = configuration["AzureAdGraph:ApiVersion"],
                ExtensionsAppClientId = configuration["AzureAdGraph:ExtensionsAppClientId"]
            };

            services.AddSingleton(azureAdGraphConfiguration);

            var azureAdB2CSettings = new AzureAdB2CSettings()
            {
                ClientId = configuration["AzureAdB2C:ClientId"],
                Tenant = configuration["AzureAdB2C:Tenant"],
                Policy = configuration["AzureAdB2C:Policy"]
            };

            services.AddSingleton(azureAdB2CSettings);

            var microsoftGraphSettings = new MicrosoftGraphSettings()
            {
                AzureAdB2CTenant = configuration["MicrosoftGraph:AzureAdB2CTenant"],
                ClientId = configuration["MicrosoftGraph:ClientId"],
                ClientSecret = configuration["MicrosoftGraph:ClientSecret"],
                ApiUrl = configuration["MicrosoftGraph:ApiUrl"],
                ApiVersion = configuration["MicrosoftGraph:ApiVersion"]
            };

            services.AddSingleton(microsoftGraphSettings);

            var cosmosDbSettings = new CosmosDbSettings()
            {
                Account = configuration["CosmosDb:Account"],
                Key = configuration["CosmosDb:Key"],
                DatabaseName = configuration["CosmosDb:DatabaseName"],
                TutorLearningProfilesContainerName = configuration["CosmosDb:TutorLearningProfilesContainerName"],
                ChatMessagesContainerName = configuration["CosmosDb:ChatMessagesContainerName"]
            };

            services.AddSingleton(cosmosDbSettings);

            var notificationHubSettings = new NotificationHubSettings()
            {
                HubName = configuration["NotificationHub:HubName"],
                HubDefaultFullSharedAccessSignature = configuration["NotificationHub:HubDefaultFullSharedAccessSignature"]
            };

            services.AddSingleton(notificationHubSettings);
        }

        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(BaseTutorRequestHandler).Assembly);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<IAzureAdGraphAuthenticationProvider, AzureAdGraphApiAuthenticationProvider>();
            services.AddScoped<IMicrosoftGraphAuthenticationProvider, MicrosoftGraphApiAuthenticationProvider>();
            services.AddHttpClient<IAdB2cGraphClientUserPropertiesProvider, AdB2cGraphClientUserPropertiesProvider>();

            var cosmosDbClient = InitializeCosmosClientInstanceAsync(configuration.GetSection("CosmosDb"))
                .GetAwaiter()
                .GetResult();

            services
                .AddSingleton<ICosmosDbClientFactory>(cosmosDbClient);

            services.AddSingleton<INotificationHubFactory, NotificationHubFactory>();
            services.AddSingleton<IPushNotificationService, PushNotificationService>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncTutorLearningProfileRepository), typeof(TutorLearningProfileRepository));
            services.AddScoped(typeof(IAsyncLessonRepository), typeof(LessonRepository));
            services.AddScoped(typeof(IAsyncRepository<ChatMessage>), typeof(ChatMessageRepository));
            services.AddScoped<IAsyncIdentityRepository<Core.Entities.User>, AzureAdIdentityRepository>();

            services.AddScoped<IAsyncTutorService, TutorService>();
            services.AddScoped<IAsyncStudentService, StudentService>();
            services.AddScoped<IAsyncUserService, UserService>();
            services.AddScoped<IAsyncChatService, ChatService>();
            services.AddScoped<IAsyncLessonTopicCategoriesService, LessonTopicCategoriesService>();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("AppDatabase")));
        }

        public static void ConfigureInvalidModelStateHandling(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var errors = string.Join(' ', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage));

                    return new BadRequestObjectResult(new Error("Model validation failed", errors));
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var azureAdB2CSettings = services.BuildServiceProvider().GetRequiredService<AzureAdB2CSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtOptions =>
                    {
                        jwtOptions.Authority = $"https://cleanarchdev.b2clogin.com/tfp/{azureAdB2CSettings.Tenant}/{azureAdB2CSettings.Policy}/v2.0/";
                        jwtOptions.Audience = azureAdB2CSettings.ClientId;
                    });
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        private static async Task<CosmosDbClientFactory> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string tutorLearningProfilesContainerName = configurationSection.GetSection("TutorLearningProfilesContainerName").Value;
            string chatMessagesContainerName = configurationSection.GetSection("ChatMessagesContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            var serializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            };

            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .WithSerializerOptions(serializerOptions)
                                .Build();
            CosmosDbClientFactory cosmosDbService = new CosmosDbClientFactory(client);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(tutorLearningProfilesContainerName, "/id");
            await database.Database.CreateContainerIfNotExistsAsync(chatMessagesContainerName, "/id");

            return cosmosDbService;
        }
    }
}
