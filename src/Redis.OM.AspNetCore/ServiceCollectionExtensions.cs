﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Redis.OM;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,
            string connectionString)
        {
            var provider = new RedisConnectionProvider(connectionString);
            return services.AddSingleton(provider);
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["REDIS_CONNECTION_STRING"];
            return services.AddSingleton(new RedisConnectionProvider(connectionString));
        }
    }
}