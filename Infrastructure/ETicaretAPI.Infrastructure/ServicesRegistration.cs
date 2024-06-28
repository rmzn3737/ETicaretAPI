
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Abstractions.Services.Configurations;
using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Abstractions.Storage.Local;
using ETicaret.Application.Abstractions.Token;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Configurations;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            //serviceCollection.AddScoped<IFileService, FileService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IApplicationService, ApplicationService>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : class, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }


        public static void AddStorage(this IServiceCollection serviceCollection, StorageType strorageType)
        {
            //serviceCollection.AddScoped<IStorage, T>();
            switch (strorageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    //serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    
                        default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;

            }
        }
    }
}
