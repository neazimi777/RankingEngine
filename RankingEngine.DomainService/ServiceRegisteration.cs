using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RankingEngine.DomainService.Abstractions;
using System.Reflection;

namespace RankingEngine.DomainService
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection ConfigureServiceRegisteration(this IServiceCollection services, IApplicationBuilder app, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assembly);

            services.AddHttpClient<IApiService, ApiService>();
            services.AddHttpClient<IJobSchedulerService, JobSchedulerService>();

            #region Hangfire Config 

            services.AddHangfire(config => config.UseMongoStorage("mongodb://localhost:27017", "HangfireDb"));

            #endregion
          
            return services;
        }
    }
}
