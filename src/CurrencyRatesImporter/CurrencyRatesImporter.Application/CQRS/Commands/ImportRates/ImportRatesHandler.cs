using CurrencyRatesImporter.Infrastructure.Services;
using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Application.CQRS.Commands.ImportRates
{
    public class ImportRatesCommandHandler
    {
        private readonly ICbrRateService _service;

        public ImportRatesCommandHandler(ICbrRateService service)
        {
            _service = service;
        }

        public async Task Handle(ImportRatesCommand command)
        {
            var date = command.Date ?? DateTime.Today;

            await _service.ProcessAsync(date);
        }
    }
}
