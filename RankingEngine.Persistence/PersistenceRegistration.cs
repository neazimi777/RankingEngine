using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RankingEngine.Persistence
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection ConfigurePersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<IRuleAccessDBContext, RuleAccessDBContext>(options =>
            //options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            #region serilogConfig

            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .Enrich.WithMachineName()
            //    .Enrich.WithEnvironmentName()
            //    .Enrich.WithThreadId()
            //    .WriteTo.Console()
            //    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) // آدرس ES
            //    {
            //        AutoRegisterTemplate = true,
            //        IndexFormat = $"myservice-logs-{DateTime.UtcNow:yyyy-MM}" // ایندکس ماهانه
            //    })
            //    .CreateLogger();
            #endregion

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IRuleRepository, RuleRepository>();

            return services;
        }
    }
}
