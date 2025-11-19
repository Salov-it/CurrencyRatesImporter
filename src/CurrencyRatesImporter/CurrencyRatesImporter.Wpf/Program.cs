using CurrencyRatesImporter.Application.DependencyInjection;
using CurrencyRatesImporter.Infrastructure.DependencyInjection;
using CurrencyRatesImporter.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
namespace CurrencyRatesImporter.Wpf
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddWpfApp();

       
            builder.Services.AddApplicationLayer();

            builder.Services.AddInfrastructureLayer(builder.Configuration);

            var host = builder.Build();

            var app = new App();
            var window = host.Services.GetRequiredService<MainWindow>();
            app.Run(window);
        }
    }
}
