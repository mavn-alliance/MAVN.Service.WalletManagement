using System;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using MAVN.Service.WalletManagement.MappingProfiles;
using MAVN.Service.WalletManagement.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MAVN.Service.WalletManagement
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "WalletManagement API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.Extend = (serviceCollection, settings) =>
                {
                    serviceCollection.AddAutoMapper(typeof(AutoMapperProfile));
                };

                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "WalletManagementLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.WalletManagementService.Db.LogsConnString;
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }
    }
}
