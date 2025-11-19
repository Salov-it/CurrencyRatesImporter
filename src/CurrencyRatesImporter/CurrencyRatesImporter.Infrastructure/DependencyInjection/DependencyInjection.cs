using CurrencyRatesImporter.Infrastructure.Common;
using CurrencyRatesImporter.Infrastructure.Common.db;
using CurrencyRatesImporter.Infrastructure.Providers.CbrRateProvider;
using CurrencyRatesImporter.Infrastructure.Repository.CbrRateRepository;
using CurrencyRatesImporter.Infrastructure.Services;
using CurrencyRatesImporter.Infrastructure.Services.CbrRateLoaderServices;
using CurrencyRatesImporter.Infrastructure.Services.ExcelGeneratorServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CurrencyRatesImporter.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,IConfiguration configuration)
        {
            var connSection = configuration.GetSection("ConnectionStrings");
            var connectionString = connSection.GetChildren().FirstOrDefault()?.Value;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string not found.");

            services.AddSingleton<ISqlConnectionFactory>(
                new SqlConnectionFactory(connectionString!)
            );

            // HttpClient для провайдера ЦБ
            services.AddHttpClient<ICbrRateProvider, CbrRateProvider>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("SolidRatesImporter/1.0");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/xml");
            });

            services.AddTransient<ICbrRateRepository, CbrRateRepository>();
            services.AddTransient<IExcelGeneratorServices, ExcelGeneratorServices>();
            services.AddTransient<ICbrRateService, CbrRateService>();


            return services;
        }
    }
}
