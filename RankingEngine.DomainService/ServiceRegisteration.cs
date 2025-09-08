using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RankingEngine.DomainService
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection ConfigureServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assembly);

          
            return services;
        }
    }
}
