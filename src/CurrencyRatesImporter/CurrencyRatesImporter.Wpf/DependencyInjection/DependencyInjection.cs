using CurrencyRatesImporter.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Wpf.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWpfApp(this IServiceCollection services)
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            return services;
        }
    }
}
