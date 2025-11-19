using CurrencyRatesImporter.Infrastructure.Providers.CbrRateProvider;
using CurrencyRatesImporter.Infrastructure.Repository.CbrRateRepository;
using CurrencyRatesImporter.Infrastructure.Services.ExcelGeneratorServices;

namespace CurrencyRatesImporter.Infrastructure.Services.CbrRateLoaderServices
{
    internal class CbrRateService : ICbrRateService
    {
        private readonly ICbrRateProvider _provider;
        private readonly ICbrRateRepository _repository;
        private readonly IExcelGeneratorServices _excel;

        public CbrRateService(ICbrRateProvider provider, ICbrRateRepository repository, IExcelGeneratorServices excel)
        {
            _provider = provider;
            _repository = repository;
            _excel = excel;
        }

        public async Task<string> ProcessAsync(DateTime date)
        {
            var xmlRates = await _provider.GetRatesAsync(date);

            if (xmlRates.Count == 0)
                throw new Exception("ЦБ РФ вернул пустой список валют.");

            await _repository.SaveRatesAsync(date, xmlRates);

            var filePath = await _excel.GenerateAsync(date, xmlRates);

            return filePath;
        }
    }
}
