using CurrencyRatesImporter.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Providers.CbrRateProvider
{
    public interface ICbrRateProvider
    {
        Task<List<CbrRateXmlModel>> GetRatesAsync(DateTime date, CancellationToken ct = default);
    }
}
