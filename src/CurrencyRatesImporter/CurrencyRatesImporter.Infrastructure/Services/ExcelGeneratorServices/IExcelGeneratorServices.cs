using CurrencyRatesImporter.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Services.ExcelGeneratorServices
{
    public interface IExcelGeneratorServices
    {
        Task<string> GenerateAsync(DateTime date, List<CbrRateXmlModel> rates);
    }
}
