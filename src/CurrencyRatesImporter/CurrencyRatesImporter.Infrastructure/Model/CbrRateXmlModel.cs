using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Model
{
   public record CbrRateXmlModel
   (
     string NumCode,
     string CharCode,
     int Nominal,
     string Name,
     decimal Value
   );
}
