using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Application.CQRS.Commands.ImportRates
{
    public record ImportRatesCommand(DateTime? Date);
}
